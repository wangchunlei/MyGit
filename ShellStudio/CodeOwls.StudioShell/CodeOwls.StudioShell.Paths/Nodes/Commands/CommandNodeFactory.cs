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
using System.Management.Automation.Runspaces;
using CodeOwls.PowerShell.Paths.Extensions;
using CodeOwls.PowerShell.Provider.Attributes;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.StudioShell.Common.Exceptions;
using CodeOwls.StudioShell.Common.IoC;
using CodeOwls.StudioShell.Common.Utility;
using CodeOwls.StudioShell.Paths.Items.Commands;
using CodeOwls.StudioShell.Paths.Utility;
using CodeOwls.PowerShell.Provider.PathNodes;
using EnvDTE80;
using Command = EnvDTE.Command;

namespace CodeOwls.StudioShell.Paths.Nodes.Commands
{
    [CmdletHelpPathID("Command")]
    public class CommandNodeFactory : NodeFactoryBase, IRemoveItem, IInvokeItem, ISetItem
    {
        private readonly Command _command;

        public CommandNodeFactory(Command command)
        {
            _command = command;
        }

        public override string Name
        {
            get
            {
                return _command.GetSafeName().MakeSafeForPath();
            }
        }

        public object RemoveItemParameters
        {
            get { return null; }
        }

        public override IPathNode GetNodeValue()
        {
            return new PathNode(new ShellCommand(_command), Name, false);
        }

        public void RemoveItem(IContext context, string path, bool recurse)
        {
            _command.Delete();

            string functionName = FunctionUtilities.GetFunctionNameFromPath(path);
            string script = "remove-item -path function:" + functionName;
            context.InvokeCommand.InvokeScript(script, false, PipelineResultTypes.None, null);
        }

        #region Implementation of IInvokeItem

        public object InvokeItemParameters
        {
            get { return null; }
        }

        public IEnumerable<object> InvokeItem(IContext provider, string path)
        {
            object ino = null;
            object outo = null;
            if (!_command.IsAvailable)
            {
                throw new InvalidOperationException("the specified command is not available at this time");
            }

            DTE2 dte = Locator.Get<DTE2>();
            if( null == dte )
            {
                throw new ServiceUnavailableException( typeof( DTE2 ) );
            }

            dte.Commands.Raise(_command.Guid, _command.ID, ref ino, ref outo);
            return null == outo ? null : new[] {outo};
        }

        #endregion

        #region Implementation of ISetItem

        public object SetItemParameters
        {
            get { return new SetItemDynamicParameters(); }
        }

        public IPathNode SetItem(IContext context, string path, object value)
        {
            var p = context.DynamicParameters as SetItemDynamicParameters;
            if (null != p && null != p.Bindings)
            {
                _command.Bindings = p.Bindings;
            }

            if (null != value)
            {
                string functionName = FunctionUtilities.GetFunctionNameFromPath(path);
                var fpath = "function:" + functionName;
                string command = String.Format(
                    @"if( test-path ""{0}"" ) 
{{ 
    remove-item -path ""{0}""; 
}} 

new-item -path ""{0}"" -value {{ {1} }} -options Constant,AllScope;",
                    fpath,
                    value
                    );
                context.InvokeCommand.InvokeScript(command, false, PipelineResultTypes.None, null);
            }
            return GetNodeValue();
        }

        #endregion

        #region Nested type: SetItemDynamicParameters

        public class SetItemDynamicParameters
        {
            [Parameter(
                HelpMessage = "The key bindings for the command"
                )]
            public string[] Bindings { get; set; }
        }

        #endregion
    }
}