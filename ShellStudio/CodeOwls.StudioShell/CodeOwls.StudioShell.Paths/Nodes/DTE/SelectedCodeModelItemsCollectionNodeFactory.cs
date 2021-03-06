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
*/using System.Collections.Generic;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Nodes.DTE
{
    internal class SelectedCodeModelItemsCollectionNodeFactory : CollectionNodeFactoryBase
    {
        private readonly DTE2 _dte;

        public SelectedCodeModelItemsCollectionNodeFactory( DTE2 dte )
        {
            _dte = dte;
        }

        public override string Name
        {
            get { return "CodeModel"; }
        }

        public override IEnumerable<PowerShell.Provider.PathNodes.INodeFactory> GetNodeChildren(IContext context)
        {
            return new INodeFactory[]
                       {
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementNamespace, "Namespace"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementClass, "Class"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementProperty, "Property"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementStruct, "Struct"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementInterface, "Interface"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementFunction, "Method"),
                           new SelectedCodeModelItemNodeFactory(_dte, vsCMElement.vsCMElementEnum, "Enum"),
                       };            
        }
    }
}
