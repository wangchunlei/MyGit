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
using System.Management.Automation;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Paths.Nodes.CodeModel;
using EnvDTE;

namespace CodeOwls.StudioShell.Paths.Nodes.ProjectModel
{
    class ProjectItemCodeModelNodeFactory : ProjectItemNodeFactory, INewItem
    {
        private readonly FileCodeModelNodeFactory _codeModelNodeFactory;

        public ProjectItemCodeModelNodeFactory(ProjectItem item) : base(item)
        {
            if( null != item.FileCodeModel )
            {
                _codeModelNodeFactory = new FileCodeModelNodeFactory( item.FileCodeModel);
            }
        }

        public override IEnumerable<INodeFactory> GetNodeChildren(IContext context)
        {
            List<INodeFactory> factories = new List<INodeFactory>();
            if (null != _item.ProjectItems)
            {
                foreach (ProjectItem item in _item.ProjectItems)
                {
                    factories.Add(new ProjectItemCodeModelNodeFactory(item));
                }
            }

            if (null != _codeModelNodeFactory )
            {
                factories.AddRange(_codeModelNodeFactory.GetNodeChildren(context));
            }

            return factories;
        }

        public IEnumerable<string> NewItemTypeNames
        {
            get
            {
                if (null == _codeModelNodeFactory)
                {
                    return null;
                }
                return _codeModelNodeFactory.NewItemTypeNames;
            }
        }

        public object NewItemParameters
        {
            get
            {
                if (null == _codeModelNodeFactory)
                {
                    return null;
                }
                return _codeModelNodeFactory.NewItemParameters;
            }
        }

        public IPathNode NewItem(IContext context, string path, string itemTypeName, object newItemValue)
        {
            if( null == _codeModelNodeFactory )
            {
                context.WriteError( 
                    new ErrorRecord( 
                        new NotSupportedException(
                            "The node at [" + path + "] does not support file code model operations"    
                        ), 
                        "StudioShell.CodeModel.NewItem",
                        ErrorCategory.InvalidOperation, 
                        path)
                        );
                return null;
            }

            return _codeModelNodeFactory.NewItem(context, path, itemTypeName, newItemValue);
        }
    }
}
