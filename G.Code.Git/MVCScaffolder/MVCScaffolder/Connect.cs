using System;
using System.Collections.Generic;
using System.IO;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace MVCScaffolder
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode,
                         object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;
            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_applicationObject.Commands;
                Microsoft.VisualStudio.CommandBars.CommandBar standardToolBar =
                  ((Microsoft.VisualStudio.CommandBars.CommandBars)
                  _applicationObject.CommandBars)["Project"];
                try
                {
                    Command command = commands.AddNamedCommand2(_addInInstance, "MVCScaffolder",
                                      "MVCScaffolder...", "Executes the command for MVCScaffolder",
                                      true, 59, ref contextGUIDS,
                                      (int)vsCommandStatus.vsCommandStatusSupported +
                                      (int)vsCommandStatus.vsCommandStatusEnabled,
                                      (int)vsCommandStyle.vsCommandStylePictAndText,
                                      vsCommandControlType.vsCommandControlTypeButton);
                    if ((command != null) && (standardToolBar != null))
                    {
                        CommandBarControl ctrl =
                          (CommandBarControl)command.AddControl(standardToolBar, 1);
                        ctrl.TooltipText = "Executes the command for MVCScaffolder";
                    }
                }
                catch (System.ArgumentException)
                {
                }
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
        /// <param term='commandName'>The name of the command to determine state for.</param>
        /// <param term='neededText'>Text that is needed for the command.</param>
        /// <param term='status'>The state of the command in the user interface.</param>
        /// <param term='commandText'>Text requested by the neededText parameter.</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (commandName == "MVCScaffolder.Connect.MVCScaffolder")
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        /// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
        /// <param term='commandName'>The name of the command to execute.</param>
        /// <param term='executeOption'>Describes how the command should be run.</param>
        /// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
        /// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
        /// <param term='handled'>Informs the caller if the command was handled or not.</param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                if (commandName == "MVCScaffolder.Connect.MVCScaffolder")
                {
                    handled = true;
                    var projects = _applicationObject.Application.Solution.Projects;
                    Project project = null;
                    foreach (Project p in projects)
                    {
                        if (p.Object is VsWebSite.VSWebSite)
                        {
                            project = (Project)p;
                            break;
                        }
                        else
                        {
                            project = (Project)p;
                            break;
                        }
                    }

                    var vsproject = (VSLangProj.VSProject)project.Object;
                    Dictionary<string, string> beDic = new Dictionary<string, string>();
                    var projectName = vsproject.Project.FullName;
                    foreach (VSLangProj.Reference reference in vsproject.References)
                    {
                        var name = reference.Name;
                        var path = reference.Path;
                        if (reference.CopyLocal)
                        {
                            string bin = @"bin\debug";
                            if (vsproject is VsWebSite.VSWebSite)
                            {
                                bin = "bin";
                            }
                            var fullName = Path.Combine(Path.GetDirectoryName(projectName), bin, name + ".dll");
                            path = fullName;
                        }
                        if (name.Contains(".BE"))
                        {
                            beDic.Add(name, path);
                            continue;
                        }
                    }
                    SelectContextFrm frm = new SelectContextFrm(beDic, projectName, vsproject);
                    frm.ShowDialog();

                    return;
                }
            }
        }
        private DTE2 _applicationObject;
        private AddIn _addInInstance;
    }
}