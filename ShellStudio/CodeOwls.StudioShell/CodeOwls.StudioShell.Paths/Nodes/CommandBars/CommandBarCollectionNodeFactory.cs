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
using System.Management.Automation;
using CodeOwls.PowerShell.Provider.Attributes;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.StudioShell.Common.Utility;
using CodeOwls.StudioShell.Paths.Items.CommandBars;
using CodeOwls.PowerShell.Provider.PathNodes;
using Microsoft.VisualStudio.CommandBars;

namespace CodeOwls.StudioShell.Paths.Nodes.CommandBars
{
    [CmdletHelpPathID("CommandBarCollection")]
    public class CommandBarCollectionNodeFactory : CollectionNodeFactoryBase, INewItem
    {
        private readonly Microsoft.VisualStudio.CommandBars.CommandBars _commandBars;

        public CommandBarCollectionNodeFactory(Microsoft.VisualStudio.CommandBars.CommandBars commandBars)
        {
            _commandBars = commandBars;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return NodeNames.CommandBars; }
        }

        public override IEnumerable<INodeFactory> Resolve(IContext context, string nodeName)
        {
            CommandBar cmdbar = null;

            try
            {
                cmdbar = _commandBars[nodeName];
            }
            catch
            {
            }

            if (null != cmdbar)
            {
                yield return new CommandBarNodeFactory(cmdbar);
            }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            List<INodeFactory> factories = new List<INodeFactory>();
            foreach (CommandBar commandBar in _commandBars)
            {
                factories.Add(new CommandBarNodeFactory(commandBar));
            }
            return factories;
        }

        #endregion

        #region Implementation of INewItem

        public IEnumerable<string> NewItemTypeNames
        {
            get { return null; }
        }

        public object NewItemParameters
        {
            get { return new NewItemDynamicParameters(); }
        }

        public IPathNode NewItem(IContext context, string path, string itemTypeName, object newItemValue)
        {
            var p = context.DynamicParameters as NewItemDynamicParameters;
            var bar = _commandBars.Add(path, p.Position, System.Type.Missing, p.Temporary.IsPresent);
            bar.Visible = true;
            return new PathNode(new ShellCommandBar(bar), path, true);
        }

        #endregion

        #region Nested type: NewItemDynamicParameters

        public class NewItemDynamicParameters
        {
            public NewItemDynamicParameters()
            {
                Position = MsoBarPosition.msoBarFloating;
            }

            [Parameter]
            public MsoBarPosition Position { get; set; }

            [Parameter]
            public SwitchParameter Temporary { get; set; }
        }

        #endregion
    }
}