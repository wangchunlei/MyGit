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
using CodeOwls.PowerShell.Provider.Attributes;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Common.Exceptions;
using CodeOwls.StudioShell.Common.IoC;
using CodeOwls.StudioShell.Common.Utility;
using CodeOwls.StudioShell.Paths.Items.ProjectModel;
using CodeOwls.StudioShell.Paths.Utility;
using EnvDTE;
using EnvDTE80;
using CodeOwls.PowerShell.Provider.PathNodes;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CodeOwls.StudioShell.Paths.Nodes.UI
{
    [CmdletHelpPathID("ErrorTaskCollection")]
    internal class ErrorListNodeFactory : CollectionNodeFactoryBase, INewItem
    {
        internal static Utility.InternalErrorListProvider ErrorProvider;

        private readonly DTE2 _dte;
        private readonly List<ErrorItem> _items;

        public ErrorListNodeFactory(ErrorItems items, DTE2 dte)
        {
            _dte = dte;
            _items = new List<ErrorItem>(items.Count);

            for (int i = 0; i < items.Count; ++i)
            {
                _items.Add(items.Item(i + 1));
            }
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return NodeNames.Errors; }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            return (from ErrorItem item in _items
                    select new ErrorListItemNodeFactory(item) as INodeFactory)
                .ToList();
        }

        #endregion

        #region Implementation of INewItem

        public IEnumerable<string> NewItemTypeNames
        {
            get { return Enum.GetNames(typeof (TaskErrorCategory)); }
        }

        public object NewItemParameters
        {
            get { return new NewItemParams(); }
        }

        public IPathNode NewItem(IContext context, string path, string itemTypeName, object newItemValue)
        {
            if (null == ErrorProvider)
            {
                var sp = Locator.Get<IServiceProvider>();
                if( null == sp )
                {
                    throw new ServiceUnavailableException( typeof( IServiceProvider ) );
                }

                ErrorProvider = new InternalErrorListProvider(sp);
            }

            var p = context.DynamicParameters as NewItemParams;

            var errorCategory = TaskErrorCategory.Error;
            try
            {
                errorCategory = (TaskErrorCategory) Enum.Parse(typeof (TaskErrorCategory), itemTypeName, true);
            }
            catch
            {
            }

            var task = new ErrorTask
                           {
                               ErrorCategory = errorCategory,
                               Text = newItemValue.ToString(),
                               Column = p.Column,
                               Line = p.Line,
                               HierarchyItem = GetHeirarchyItemFromSourcePath(context, p.SourcePath),
                               Document = GetDocumentFromSourcePath(context, p.SourcePath)
                           };

            task.Navigate += NavigateToTask;
            ErrorProvider.Tasks.Add(task);
            ErrorProvider.Show();

            var errors = _dte.ToolWindows.ErrorList.ErrorItems;

            ErrorItem item = errors.Item(errors.Count);
            if (null != item)
            {
                return new ErrorListItemNodeFactory(item).GetNodeValue();
            }

            return null;
        }

        private class NewItemParams
        {
            [Parameter(ParameterSetName = "nameSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "pathSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ValueFromPipelineByPropertyName = true)]
            public int Line { get; set; }

            [Parameter(ParameterSetName = "nameSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "pathSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ValueFromPipelineByPropertyName = true)]
            public int Column { get; set; }

            [Parameter(ParameterSetName = "nameSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "pathSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ValueFromPipelineByPropertyName = true)]
            [Alias("File", "ProjectItem")]
            public string SourcePath { get; set; }
        }

        #endregion

        private IVsHierarchy GetHeirarchyItemFromSourcePath(IContext context, string sourcePath)
        {
            IVsSolution sln = Locator.GetService<IVsSolution>();
            if (null == sln)
            {
                return null;
            }

            Project project = GetProjectFromSourcePath(context, sln, sourcePath);
            if (null == project)
            {
                return null;
            }

            IVsHierarchy heirarchy;
            sln.GetProjectOfUniqueName(project.UniqueName, out heirarchy);

            return heirarchy;
        }

        private Project GetProjectFromSourcePath(IContext context, IVsSolution sln, string sourcePath)
        {
            if (String.IsNullOrEmpty(sourcePath))
            {
                return null;
            }

            var paths = context.SessionState.Path.GetResolvedPSPathFromPSPath(sourcePath);
            sourcePath = paths[0].Path;
            var nodeFactory = context.ResolvePath(sourcePath);
            var item = nodeFactory.GetNodeValue().Item;
            var p = item as ShellProject;
            var i = item as ShellProjectItem;

            if (null != p)
            {
                return p.Object as Project;
            }

            if (null != i)
            {
                return i.ContainingProject.AsProject();
            }

            return null;
        }

        private string GetDocumentFromSourcePath(IContext context, string sourcePath)
        {
            if (String.IsNullOrEmpty(sourcePath))
            {
                return null;
            }

            var paths = context.SessionState.Path.GetResolvedPSPathFromPSPath(sourcePath);
            sourcePath = paths[0].Path;
            var nodeFactory = context.ResolvePath(sourcePath);
            var item = nodeFactory.GetNodeValue().Item;
            var i = item as ShellProjectItem;

            if (null != i)
            {
                return i.FileNames[0];
            }

            return sourcePath;
        }

        private static void NavigateToTask(object sender, EventArgs e)
        {
            var task = sender as ErrorTask;
            if (null == task || null == ErrorProvider)
            {
                return;
            }
            try
            {
                ErrorProvider.Navigate(task, new Guid(EnvDTE.Constants.vsViewKindCode));
            }
            catch
            {
            }
        }
    }
}