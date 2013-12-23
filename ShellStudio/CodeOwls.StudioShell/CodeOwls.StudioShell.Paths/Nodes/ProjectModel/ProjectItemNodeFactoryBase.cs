/*
   Copyright (c) 2011 Code Owls LLC, All Rights Reserved.

   Licensed under the Microsoft Reciprocal License (Ms-RL) (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.opensource.org/licenses/ms-rl

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using CodeOwls.PowerShell.Paths.Exceptions;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Common.Configuration;
using CodeOwls.StudioShell.Paths.Items;
using CodeOwls.StudioShell.Paths.Items.ProjectModel;
using CodeOwls.StudioShell.Paths.Nodes.CodeModel;
using CodeOwls.StudioShell.Paths.Nodes.PropertyModel;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Nodes.ProjectModel
{
    public abstract class ProjectItemNodeFactoryBase : NodeFactoryBase, IRemoveItem, IRenameItem, ICopyItem
    {
        protected readonly ProjectItem _item;
        
        protected ProjectItemNodeFactoryBase(ProjectItem item)
        {
            _item = item;
        }

        public override string Name
        {
            get { return _item.Name; }
        }

        protected bool IsContainer
        {
            get
            {
                bool hasItems = null != _item.ProjectItems;
                bool hasCodeModel = null != _item.FileCodeModel;

                return hasItems || hasCodeModel;
            }
        }

        #region Implementation of IRemoveItem

        public object RemoveItemParameters
        {
            get { return null; }
        }

        public void RemoveItem(IContext context, string path, bool recurse)
        {
            if (context.Force)
            {
                _item.Delete();
            }
            else
            {
                _item.Remove();
            }
        }

        #endregion

        
        #region Implementation of IRenameItem

        public object RenameItemParameters
        {
            get { return null; }
        }

        public void RenameItem(IContext context, string path, string newName)
        {
            _item.Name = newName;
        }

        #endregion

        #region Implementation of ICopyItem

        public object CopyItemParameters
        {
            get { return null; }
        }

        public IPathNode CopyItem(IContext context, string path, string copyPath, IPathNode destinationContainer,
                                  bool recurse)
        {
            ProjectItems destinationItems = null;
            ShellProject destinationProject = destinationContainer.Item as ShellProject;
            ShellProjectItem destinationItem = destinationContainer.Item as ShellProjectItem;

            if (null == destinationProject)
            {
                var containerKinds = new[]
                                         {
                                             Constants.vsProjectItemKindPhysicalFolder,
                                             Constants.vsProjectItemsKindSolutionItems,
                                             Constants.vsProjectItemKindSubProject,
                                             Constants.vsProjectItemKindVirtualFolder,
                                         };

                if (null == destinationItem || !containerKinds.Contains(destinationItem.Kind))
                {
                    throw new InvalidOperationException(
                        "Project items can only be moved or copied into a project node, a project folder, or solution folder.");
                }

                destinationItems = destinationItem.AsProjectItem().ProjectItems;
                destinationProject = destinationItem.ContainingProject;
            }
            else
            {
                destinationItems = destinationProject.AsProject().ProjectItems;
            }

            ShellProject sourceProject = new ShellProject(_item.ContainingProject);

            bool isWithinProject = sourceProject.UniqueName == destinationProject.UniqueName;

            if (String.IsNullOrEmpty(copyPath))
            {
                if (isWithinProject)
                {
                    copyPath = "Copy of " + path;
                }
                else
                {
                    copyPath = path;
                }
            }

            ProjectItem newItem = null;
            CopyItemContext copyContext = null;
            var events = ((Events2) _item.DTE.Events);
            copyContext = new CopyItemContext(_item, copyPath, destinationItems, isWithinProject, recurse);

            events.ProjectItemsEvents.ItemAdded += copyContext.OnItemAdded;
            Exception error = null;
            AddItemCopy(copyContext, out error);

            events.ProjectItemsEvents.ItemAdded -= copyContext.OnItemAdded;
            if (null != error)
            {
                context.WriteError(new ErrorRecord(
                                       new CopyOrMoveItemInternalException(path, copyContext.CopyPath, error),
                                       "StudioShell.CopyItem.Internal", ErrorCategory.WriteError, path));
                return null;
            }
            return new PathNode(ShellObjectFactory.CreateFromProjectItem(newItem), copyPath, false);
        }

        private void AddItemCopy(CopyItemContext copyItemContext, out Exception e)
        {
            e = null;
            try
            {
                switch (copyItemContext.Item.Kind)
                {
                    case (Constants.vsProjectItemKindSolutionItems):
                    case (Constants.vsProjectItemKindPhysicalFile):
                        {
                            for (short i = 1; i <= copyItemContext.Item.FileCount; ++i)
                            {
                                var file = copyItemContext.Item.get_FileNames(i);
                                var newFileName = copyItemContext.CopyPath;

                                if (copyItemContext.IsWithinProject)
                                {
                                    copyItemContext.DestinationItems.AddFromTemplate(file, newFileName);
                                }
                                else
                                {
                                    copyItemContext.DestinationItems.AddFromFileCopy(file);
                                    copyItemContext.Root.LastCreatedItem.Name = newFileName;
                                }
                            }
                            break;
                        }
                    case (Constants.vsProjectItemKindPhysicalFolder):
                        copyItemContext.DestinationItems.AddFolder(copyItemContext.CopyPath,
                                                                   Constants.vsProjectItemKindPhysicalFolder);
                        break;
                    case (Constants.vsProjectItemKindVirtualFolder):
                        copyItemContext.DestinationItems.AddFolder(copyItemContext.CopyPath,
                                                                   Constants.vsProjectItemKindVirtualFolder);
                        break;
                    case (Constants.vsProjectItemKindSubProject):
                    default:
                        break;
                }
            }
            catch (Exception ce)
            {
                e = ce;
            }

            if (!copyItemContext.Recurse || null != e)
            {
                return;
            }

            var clone = copyItemContext.Clone();
            clone.DestinationItems = copyItemContext.Root.LastCreatedItem.ProjectItems;
            foreach (ProjectItem subItem in copyItemContext.Item.ProjectItems)
            {
                clone.Item = subItem;
                clone.CopyPath = clone.Item.Name;
                AddItemCopy(clone, out e);
            }
        }

        private void RecurseProjectItems(List<ProjectItem> items, ProjectItem projectItem)
        {
            if (null != projectItem &&
                null != projectItem.ProjectItems &&
                0 != projectItem.ProjectItems.Count)
            {
                items.AddRange(projectItem.ProjectItems.Cast<ProjectItem>());

                foreach (ProjectItem pi in projectItem.ProjectItems)
                {
                    RecurseProjectItems(items, pi);
                }
            }
        }

        #endregion

        public override IPathNode GetNodeValue()
        {
            var item = ShellObjectFactory.CreateFromProjectItem(_item);
            return new PathNode(item, Name, IsContainer);
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            List<INodeFactory> factories = new List<INodeFactory>();
            if (null != _item.ProjectItems)
            {
                foreach (ProjectItem item in _item.ProjectItems)
                {
                    factories.Add(ProjectNodeFactory.Create(item));
                }
            }

            if (PathTopologyVersions.SupportsProjectItemCodeModel( context ))
            {
                if (null != _item.FileCodeModel)
                {
                    factories.Add(new FileCodeModelNodeFactory(_item.FileCodeModel));
                }
            }

            if (null != _item.Properties)
            {
                factories.Add(new PropertyCollectionNodeFactory("ItemProperties", _item.Properties));
            }

            return factories;
        }

        #region Nested type: CopyItemContext

        public class CopyItemContext
        {
            public CopyItemContext(ProjectItem item, string copyPath, ProjectItems destinationItems,
                                   bool isWithinProject, bool recurse)
            {
                Item = item;
                CopyPath = copyPath;
                DestinationItems = destinationItems;
                IsWithinProject = isWithinProject;
                Recurse = recurse;
                Root = this;
            }

            public CopyItemContext Root { get; private set; }

            public ProjectItem RootNewItem { get; set; }
            public ProjectItem Item { get; set; }

            public string CopyPath { get; set; }

            public ProjectItems DestinationItems { get; set; }

            public bool IsWithinProject { get; set; }

            public bool Recurse { get; set; }

            public ProjectItem LastCreatedItem { get; set; }

            public CopyItemContext Clone()
            {
                return new CopyItemContext(Item, CopyPath, DestinationItems, IsWithinProject, Recurse) {Root = Root};
            }

            public void OnItemAdded(ProjectItem a)
            {
                if (null == RootNewItem)
                {
                    RootNewItem = a;
                }

                Root.LastCreatedItem = a;
            }
        }

        #endregion
    }
}
