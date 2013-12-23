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

namespace CodeOwls.StudioShell.Paths.Nodes.PropertyModel
{
    internal class PropertyCollectionNodeFactory : CollectionNodeFactoryBase
    {
        private readonly string _name;
        private readonly Properties _properties;

        public PropertyCollectionNodeFactory(Properties properties) : this(NodeNames.Properties, properties)
        {
        }

        public PropertyCollectionNodeFactory(string name, Properties properties)
        {
            _name = name;
            _properties = properties;
        }

        public override string Name
        {
            get { return _name; }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            foreach (Property property in _properties)
            {
                yield return new PropertyNodeFactory(property);
            }
        }
    }
}