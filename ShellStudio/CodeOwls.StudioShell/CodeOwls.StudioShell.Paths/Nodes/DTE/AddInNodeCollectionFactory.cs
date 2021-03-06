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
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.StudioShell.Common.Utility;
using EnvDTE;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace CodeOwls.StudioShell.Paths.Nodes.DTE
{
    public class AddInNodeCollectionFactory : CollectionNodeFactoryBase
    {
        private readonly AddIns _addIns;

        public AddInNodeCollectionFactory(AddIns addIns)
        {
            _addIns = addIns;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return NodeNames.AddIns; }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            List<INodeFactory> factories = new List<INodeFactory>();
            foreach (AddIn addIn in _addIns)
            {
                factories.Add(new AddInNodeFactory(addIn));
            }
            return factories;
        }

        #endregion
    }
}