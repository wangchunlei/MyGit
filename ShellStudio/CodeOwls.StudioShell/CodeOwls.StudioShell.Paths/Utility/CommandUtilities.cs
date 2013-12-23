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
using System.Linq;
using System.Management.Automation;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.StudioShell.Paths.Items.Commands;
using CodeOwls.StudioShell.Paths.Nodes.Commands;
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Utility
{
    internal class CommandUtilities
    {
        internal static ShellCommand GetOrCreateCommand(IContext context, DTE2 dte, string caption,
                                                        object commandPathOrScript)
        {
            var stringValue = commandPathOrScript as string;
            var script = commandPathOrScript as ScriptBlock;

            ShellCommand cmd = null;
            if (null != stringValue)
            {
                // locate an existing command by path
                try
                {
                    var command = context.ResolvePath(stringValue);
                    if (null != command)
                    {
                        var value = command.GetNodeValue();
                        if (null != value)
                        {
                            cmd = value.Item as ShellCommand;
                        }
                    }
                }
                catch
                {
                }

                // locate an existing command by name
                if (null == cmd)
                {
                    var node = context.ResolvePath("dte:/commands");
                    var factories = node.Resolve(context, stringValue);
                    if (null != factories && factories.Any())
                    {
                        var value = factories.First().GetNodeValue();
                        if (null != value)
                        {
                            cmd = value.Item as ShellCommand;
                        }
                    }
                }

                // assume the string is script;
                if (null == cmd)
                {
                    script = ScriptBlock.Create(stringValue);
                }
            }

            if (null == cmd && null != script)
            {
                var coll =
                    new CommandCollectionPathNodeFactory(dte.Commands as Commands2);
                var pm = new CommandCollectionPathNodeFactory.NewItemDynamicParameters
                             {
                                 Button = true,
                                 Label = caption,
                                 Supported = true,
                                 Enabled = true
                             };
                cmd =
                    coll.NewItem(
                        new Context(context, pm),
                        "anonymousCommand_" + Guid.NewGuid().ToString("N"),
                        null, script
                        ).Item as ShellCommand;
            }
            return cmd;
        }
    }
}
