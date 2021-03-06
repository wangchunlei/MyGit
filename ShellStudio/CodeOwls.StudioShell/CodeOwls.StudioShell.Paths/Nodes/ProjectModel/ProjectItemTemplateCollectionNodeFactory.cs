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


using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Paths.Items.Templates;

namespace CodeOwls.StudioShell.Paths.Nodes.ProjectModel
{
    public class NamedItemCollectionNodeFactory : CollectionNodeFactoryBase
    {
        private readonly IEnumerable<INodeFactory> _items;
        private readonly string _name;

        public NamedItemCollectionNodeFactory(string name, IEnumerable<INodeFactory> items)
        {
            _name = name;
            _items = items;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return _name; }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            return _items;
        }

        #endregion
    }

    public class NamedItemNodeFactory : NodeFactoryBase
    {
        private readonly string _name;
        private readonly object _value;

        public NamedItemNodeFactory(string name, object value)
        {
            _name = name;
            _value = value;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return _name; }
        }

        public override IPathNode GetNodeValue()
        {
            return new PathNode(_value, Name, false);
        }

        #endregion
    }

    public class TemplateCollectionNodeFactory : NamedItemCollectionNodeFactory
    {
        public TemplateCollectionNodeFactory(DirectoryInfo projectItemTemplateRoot, DirectoryInfo projectTemplateRoot)
            : base("Templates", new[]
                                    {
                                        new ProjectItemLanguageTemplateCollectionNodeFactory(projectItemTemplateRoot,
                                                                                             "ProjectItems"),
                                        new ProjectItemLanguageTemplateCollectionNodeFactory(projectTemplateRoot,
                                                                                             "Projects"),
                                        new ProjectItemLanguageTemplateCollectionNodeFactory(projectItemTemplateRoot,
                                                                                             "ProjectItemsCache"),
                                        new ProjectItemLanguageTemplateCollectionNodeFactory(projectTemplateRoot,
                                                                                             "ProjectsCache"),
                                    })
        {
        }
    }

    public class ProjectItemLanguageTemplateCollectionNodeFactory : CollectionNodeFactoryBase
    {
        private readonly string _name;
        private readonly DirectoryInfo _templateRoot;

        public ProjectItemLanguageTemplateCollectionNodeFactory(DirectoryInfo templateRoot, string name)
        {
            _templateRoot = templateRoot;
            _name = name;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return _name; }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            return (from dir in _templateRoot.GetDirectories()
                    select new ProjectItemTemplateCollectionNodeFactory(dir, dir.Name)).Cast<INodeFactory>();
        }

        #endregion
    }

    public class ProjectItemTemplateCollectionNodeFactory : CollectionNodeFactoryBase
    {
        private readonly string _name;
        private readonly DirectoryInfo _templateRoot;

        public ProjectItemTemplateCollectionNodeFactory(DirectoryInfo templateRoot, string name)
        {
            _templateRoot = templateRoot;
            _name = name;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return _name; }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            string languageName = Path.GetDirectoryName(_templateRoot.FullName);
            return (from file in _templateRoot.GetFiles("*.zip", SearchOption.AllDirectories)
                    let t = new ShellTemplate(Path.GetFileNameWithoutExtension(file.FullName), languageName)
                    orderby t.Name
                    select new NamedItemNodeFactory(t.Name, t)).Cast<INodeFactory>();
        }

        #endregion
    }
}