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
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Reflection;
using System.Text;
using CodeOwls.PowerShell.Paths.Processors;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.StudioShell.Common.IoC;
using CodeOwls.StudioShell.Common.Utility;
using CodeOwls.StudioShell.Provider.Variables;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Provider
{
    [CmdletProvider("PSDTE", ProviderCapabilities.ShouldProcess)]
    public class DTEProvider : CodeOwls.PowerShell.Provider.Provider
    {
        private DTEDrive DTEDrive
        {
            get
            {
                var drive = PSDriveInfo as DTEDrive;

                if (null == drive)
                {
                    drive = ProviderInfo.Drives.FirstOrDefault() as DTEDrive;
                }

                return drive;
            }
        }

        private static DTE2 DTE2
        {
            get { return Locator.Get<DTE2>(); }
        }

        protected override PowerShell.Provider.PathNodeProcessors.IContext CreateContext(string path, bool recurse, bool resolveFinalNodeFilterItems)
        {
            var context = new Context( this, path, PSDriveInfo, PathNodeProcessor, DynamicParameters, DTEDrive.PathTopologyVersion, recurse );
            context.ResolveFinalNodeFilterItems = resolveFinalNodeFilterItems;
            return context;
        }

        protected override IPathNodeProcessor PathNodeProcessor
        {
            get { return new PathNodeProcessor( DTE2 ); }
        }

        protected override ProviderInfo Start(ProviderInfo providerInfo)
        {
            ConfigureRunspace( this.SessionState );

            return base.Start(providerInfo);
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            if( drive is DTEDrive )
            {
                return drive;
            }

            return new DTEDrive( drive, DTE2 );
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            return new Collection<PSDriveInfo>(
                new List<PSDriveInfo>
                    {
                        new DTEDrive(
                            new PSDriveInfo("DTE", ProviderInfo, String.Empty, "DTE Drive", null),
                            DTE2
                            )
                    }
                );
        }

        void ConfigureRunspace(SessionState sessionState)
        {
            AddRunspaceVariables(sessionState);
        }

        static Version GetAssemblyFileVersion()
        {
            return (
                from AssemblyFileVersionAttribute attr in
                    Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
                select new Version(attr.Version.Replace(".*", ""))
            ).First();
        }

        private void AddRunspaceVariables(SessionState sessionState)
        {
            try
            {
                sessionState.InvokeCommand.InvokeScript(
                    String.Format(
                        "$psversiontable['StudioShellVersion'] = [version]'{0}'",
                        GetAssemblyFileVersion()
                        )
                    );
            }
            catch (Exception e)
            {
                
            }

            PSVariable[] psv = GetStudioShellPSVariables();

            var warns = new List<string>();
            psv.ToList().ForEach(v =>
                                     {
                                         if (null != sessionState.PSVariable.Get(v.Name))
                                         {
                                             if ("dte" != v.Name)
                                             {
                                                 warns.Add(v.Name);
                                             }
                                             return;
                                         }

                                         sessionState.PSVariable.Set(v);
                                     });
            if (warns.Any())
            {
                sessionState.InvokeCommand.InvokeScript(
                    String.Format(
                        "write-debug 'The following StudioShell variables could not be defined because the variables names are already in use: `${0}'",
                        String.Join(", `$", warns.ToArray())
                        )
                    );
            }
        }

        private static DTEEventSource EventSource;

        private PSVariable[] GetStudioShellPSVariables()
        {
            var dte2 = DTEProvider.DTE2;
            var events = dte2.Events as Events2;
            if (null == EventSource)
            {
                EventSource = new DTEEventSource(events);
            }

            return new PSVariable[]
	                   {
	                       new PSVariable("dte", dte2),

                           // adding each individual event container as a unique variable seems silly,
                           //   but it is apparantly necessary to have the types recognized as event-supporting
                           //   .NET COM wrappers in the powershell session
                           new PSVariable("events", events),
	                       new PSVariable("solutionEvents", EventSource.SolutionEvents as SolutionEvents),
	                       new PSVariable("selectionEvents", EventSource.SelectionEvents as SelectionEvents),
                           new PSVariable( "projectEvents", EventSource.ProjectEvents as ProjectsEvents ),
                           new PSVariable( "projectItemEvents", EventSource.ProjectItemsEvents as ProjectItemsEvents ),
                           new PSVariable( "buildEvents", EventSource.BuildEvents),
                           new PSVariable("debuggerEvents", EventSource.DebuggerEvents),
                           new PSVariable("debuggerProcessEvents", EventSource.DebuggerProcessEvents),
                           new PSVariable("debuggerExpressionEvaluationEvents", EventSource.DebuggerExpressionEvaluationEvents),
                           new PSVariable("dteEvents", EventSource.DteEvents),
                           new PSVariable("findEvents", EventSource.FindEvents),
                           new PSVariable("miscFilesEvents", EventSource.MiscFileEvents),
                           new PSVariable("publishEvents", EventSource.PublishEvents),
                           new PSVariable("solutionItemsEvents", EventSource.SolutionItemEvents),
                           
	                       new ActiveWindowVariable(dte2, "activeWindow"),
	                       new SelectedProjectItems(dte2, "selectedProjectItems"),
	                       new SelectedProjects(dte2, "selectedProjects"),
	                       new CurrentDebugModeVariable(dte2),
	                       new CurrentProcessVariable(dte2),
	                       new CurrentStackFrameVariable(dte2),
	                       new CurrentThreadVariable(dte2),
	                       new SelectedCodeElementVariable(dte2, "selectedClass", vsCMElement.vsCMElementClass),
                           new SelectedCodeElementVariable(dte2, "selectedMethod", vsCMElement.vsCMElementFunction),
                           new SelectedCodeElementVariable(dte2, "selectedProperty", vsCMElement.vsCMElementProperty),
                           new SelectedCodeElementVariable(dte2, "selectedNamespace", vsCMElement.vsCMElementNamespace),
	                   };
        }

    }
}
