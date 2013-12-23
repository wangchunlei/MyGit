
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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using CodeOwls.PowerShell.Host;
using CodeOwls.PowerShell.Host.Configuration;
using CodeOwls.PowerShell.Host.Executors;
using CodeOwls.PowerShell.WinForms;
using CodeOwls.StudioShell.Common.Configuration;
using CodeOwls.StudioShell.Common.IoC;
using CodeOwls.StudioShell.Host;
using CodeOwls.StudioShell.Provider;
using CodeOwls.StudioShell.Provider.Variables;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Globalization;
using CodeOwls.StudioShell.Configuration;
using CodeOwls.PowerShell.WinForms.Utility;
using CodeOwls.StudioShell.Utility;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using stdole;
using Command=EnvDTE.Command;
using Debugger = System.Diagnostics.Debugger;
using ICommandExecutor = CodeOwls.PowerShell.Host.Executors.ICommandExecutor;

namespace CodeOwls.StudioShell
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{
        static Connect()
        {
            EnvironmentConfiguration.UpdateEnvironmentForHost();
        }

		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
		    AddIn addIn = addInInst as AddIn;
		    var name = addIn.Name;
		    var progid = addIn.ProgID;
            
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;
            _statusBarAnimation = new StatusBarAnimationState(_applicationObject);            

            var sp = new ServiceProvider(_applicationObject as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);

		    Locator.Set<DTE2>(_applicationObject);
		    Locator.Set<AddIn>(_addInInstance);
		    Locator.Set<IServiceProvider>(sp);

            if( null == ApplicationObject )
            {
                ApplicationObject = _applicationObject;
            }

            if( null == AddInInstance)
            {
                AddInInstance = _addInInstance;
            }

            switch( connectMode )
            {
                case( ext_ConnectMode.ext_cm_UISetup):
                    RunUISetup();
                    break;
                
                case (ext_ConnectMode.ext_cm_Startup):
                case( ext_ConnectMode.ext_cm_AfterStartup):
                    RemoveScriptCommands();
                    if (Settings.RunStudioShellOnStartup)
                    {
                        ExecuteStudioShellCommand();
                    }
                    break;
                
                default:
                    break;
            }            
		}

        void RemoveScriptCommands()
        {
            var dte = _applicationObject;
            if( null == dte)
            {
                return;
            }

            var keepers = new[] {StudioShellExecuteCommandName, StudioShellRestartCommandName, StudioShellCommandName, StudioShellCancelCommandName};
            List<Command> losers = new List<Command>();

            foreach( Command cmd in dte.Commands )
            {
                if( keepers.Contains( cmd.Name, StringComparer.InvariantCultureIgnoreCase ))
                {
                    continue;
                }

                if( FunctionUtilities.IsPowerShellCommandName( cmd.Name ))
                {
                    losers.Add(cmd);
                }
            }

            losers.ForEach( cmd => cmd.Delete() );
        }

	    private void RunUISetup()
	    {
	        string toolsMenuName;

	        try
	        {
	            //If you would like to move the command to a different menu, change the word "Tools" to the 
	            //  English version of the menu. This code will take the culture, append on the name of the menu
	            //  then add the command to that menu. You can find a list of all the top-level menus in the file
	            //  CommandBar.resx.
	            string resourceName;
	            ResourceManager resourceManager = new ResourceManager(StudioshellResources, Assembly.GetExecutingAssembly());
	            CultureInfo cultureInfo = new CultureInfo(_applicationObject.LocaleID);
					
	            if(cultureInfo.TwoLetterISOLanguageName == "zh")
	            {
	                System.Globalization.CultureInfo parentCultureInfo = cultureInfo.Parent;
	                resourceName = String.Concat(parentCultureInfo.Name, "View");
	            }
	            else
	            {
	                resourceName = String.Concat(cultureInfo.TwoLetterISOLanguageName, "View");
	            }
	            toolsMenuName = resourceManager.GetString(resourceName);
	        }
	        catch
	        {
	            //We tried to find a localized version of the word Tools, but one was not found.
	            //  Default to the en-US word, which may work for the current culture.
	            toolsMenuName = "View";
	        }
           
            //Place the command on the tools menu.
	        //Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
	        CommandBar menuBarCommandBar = ((CommandBars)_applicationObject.CommandBars)["MenuBar"];

	        //Find the Tools command bar on the MenuBar command bar:
	        CommandBarControl viewControl = menuBarCommandBar.Controls[toolsMenuName];
	        CommandBarPopup viewPopup = (CommandBarPopup)viewControl;

	        //This try/catch block can be duplicated if you wish to add multiple commands to be handled by your Add-in,
	        //  just make sure you also update the QueryStatus/Exec method to include the new command names.
	        try
	        {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_applicationObject.Commands;

                SafeDeleteCommand("CodeOwls.StudioShell.Connect.StudioShell");

	            Command command = commands.AddNamedCommand2(_addInInstance, 
	                                                        "StudioShell", "StudioShell", "Opens Studio Shell", 
	                                                        false, 1, 
	                                                        ref contextGUIDS, 
	                                                        (int)vsCommandStatus.vsCommandStatusSupported+(int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
	            command.Bindings = new object[] {"Global::Ctrl+Shift+Enter"};

	            //Add a control for the command to the tools menu:
	            if((command != null) && (viewPopup != null))
	            {
	                command.AddControl(viewPopup.CommandBar, 1);
	            }

                SafeDeleteCommand("CodeOwls.StudioShell.Connect.Execute");
	            command = commands.AddNamedCommand2(_addInInstance,
	                                                "Execute", "Exceute", "Executes PowerShell from a Macro",
	                                                false, 0,
	                                                ref contextGUIDS,
	                                                (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, 
	                                                (int)vsCommandStyle.vsCommandStyleText, 
	                                                vsCommandControlType.vsCommandControlTypeButton);

                SafeDeleteCommand("CodeOwls.StudioShell.Connect.Cancel");
	            command = commands.AddNamedCommand2(_addInInstance,
	                                                "Cancel", "Cancel Executing Pipeline", "Cancels the Currently Executing Studio Shell Pipeline",
	                                                false, 2,
	                                                ref contextGUIDS,
	                                                (int)vsCommandStatus.vsCommandStatusSupported,
	                                                (int)vsCommandStyle.vsCommandStylePictAndText,
	                                                vsCommandControlType.vsCommandControlTypeButton);
	            command.Bindings = new object[] { "Global::Ctrl+Shift+C" };                    

	            if ((command != null) && (viewPopup != null))
	            {
	                command.AddControl(viewPopup.CommandBar, 2);
	            }

                SafeDeleteCommand(StudioShellRestartCommandName);
                command = commands.AddNamedCommand2(_addInInstance,
                                                    "ResetRunspace", "Reset StudioShell", "Resets the StudioShell Runspace",
                                                    false, 3,
                                                    ref contextGUIDS,
                                                    (int)vsCommandStatus.vsCommandStatusSupported,
                                                    (int)vsCommandStyle.vsCommandStylePictAndText,
                                                    vsCommandControlType.vsCommandControlTypeButton);

                if ((command != null) && (viewPopup != null))
                {
                    command.AddControl(viewPopup.CommandBar, 2);
                }
	        }
	        catch(System.ArgumentException)
	        {
	            //If we are here, then the exception is probably because a command with that name
	            //  already exists. If so there is no need to recreate the command and we can 
	            //  safely ignore the exception.
	        }
	    }

	    private void SafeDeleteCommand(string cmdName)
	    {
	        try
	        {
	            _applicationObject.Commands.Item( cmdName ).Delete();
	        }
	        catch
	        {
	        }
	    }

	    /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
            var shell = Shell;

            if (null != shell)
            {
                shell.Stop();
                shell.Dispose();
            }

	        RemoveScriptCommands();
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
            if (_toolWindow != null)
            {
                _toolWindow.Close(vsSaveChanges.vsSaveChangesYes);
                _toolWindow = null;
            }
            if (_consoleControl != null)
            {
                _consoleControl.Dispose();
                _consoleControl = null;
            }
            if (null != Shell)
            {
                Shell.Stop();
                Shell.Dispose();
                Shell = null;
            }
		}
		
		/// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
		/// <param term='commandName'>The name of the command to determine state for.</param>
		/// <param term='neededText'>Text that is needed for the command.</param>
		/// <param term='status'>The state of the command in the user interface.</param>
		/// <param term='commandText'>Text requested by the neededText parameter.</param>
		/// <seealso class='Exec' />
		public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
		{
			if(neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
			{
				if(commandName == StudioShellCommandName)
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
					return;
				}

                if( commandName == StudioShellRestartCommandName )
                {
                    if (null != Shell )
                    {
                        status = (vsCommandStatus)(vsCommandStatus.vsCommandStatusSupported |
                                  vsCommandStatus.vsCommandStatusEnabled);
                    }
                    else
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported;
                    }
                    return;
                }
                if (commandName == StudioShellCancelCommandName)
                {
                    if( null != Shell && Shell.CurrentState == CommandExecutorState.Unavailable )
                    {
                        status = (vsCommandStatus) ( vsCommandStatus.vsCommandStatusSupported |
                                  vsCommandStatus.vsCommandStatusEnabled);
                    }
                    else
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported;
                    }
                    return;
                }

                if (commandName == StudioShellExecuteCommandName ||
                    FunctionUtilities.IsPowerShellCommandName(commandName))
                {
                    if (null != Shell && Shell.CurrentState == CommandExecutorState.Unavailable)
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported;
                    }
                    else
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported |
                                 vsCommandStatus.vsCommandStatusEnabled;
                    }
                    return;
                }

                if( commandName == StudioShellDoNothingCommandName )
                {
                    status = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
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
			if(executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
                if (commandName == StudioShellCommandName)
				{
                    ExecuteStudioShellCommand();

				    handled = true;
					return;
				}

                if( commandName == StudioShellRestartCommandName)
                {
                    RestartShell();
                    handled = true;
                    return;
                }

                if (commandName == StudioShellExecuteCommandName)
                {
                    varOut = ExecuteStudioShellExecuteCommand(varIn);
                    handled = true;
                    return;
                }

                if (commandName == StudioShellCancelCommandName)
                {
                    ExecuteStudioShellCancelCommand();
                    handled = true;
                    return;
                }

                if (commandName.EndsWith( "_1"))
                {
                    handled = true;
                    return;
                }

                if (FunctionUtilities.IsPowerShellCommandName(commandName))
                {
                    varOut = null;
                    handled = true;
                    object input = varIn;
                    ThreadPool.QueueUserWorkItem( ctx=> ExecuteStudioShellPowerShellCommand(commandName, input) );
                    return;
                }
			}

            
		}

	    private object ExecuteStudioShellPowerShellCommand(string commandName, object varIn)
	    {
	        object varOut = null;
	        if (null != Shell)
	        {
	            string command = commandName;
	            var args = new Dictionary<string, object>();
	            args["varIn"] = varIn;
	            varOut = Shell.Execute(command, args);
	        }
	        return varOut;
	    }

	    private void ExecuteStudioShellCancelCommand()
	    {
	        if (null != Executor)
	        {
	            if( ! Executor.CancelCurrentExecution( 3000 ) )
	            {
	                PromptForForceCommandCancel();
	            }
	        }
	    }

	    private static void PromptForForceCommandCancel()
	    {
	        var result = MessageBox.Show(
	            @"The currently executing pipeline has not responded to the stop request.\n\nPress OK to continue waiting, or Cancel to destroy and recreate the runspace.",
	            @"Cancel executing pipeline",
	            MessageBoxButtons.OKCancel,
	            MessageBoxIcon.Warning,
	            MessageBoxDefaultButton.Button1
	            );

	        if (DialogResult.Cancel == result)
	        {
	            Shell.Stop(true);
	            Shell.Run();
	        }
	    }

	    private object ExecuteStudioShellExecuteCommand(object varIn)
	    {
	        object varOut = null;
	        if( null != Executor )
	        {
	            string command = varIn.ToString();
	            varOut = Executor.Execute(command);
	        }
	        return varOut;
	    }

	    private void ExecuteStudioShellCommand()
	    {
	        if (null == Shell)
	        {
	            var resourceManager = new ResourceManager(StudioshellResources,
	                                                      Assembly.GetExecutingAssembly());
	            _statusBarAnimation = new StatusBarAnimationState(_applicationObject);

	            switch (Settings.ConsoleChoice)
	            {
	                case (ConsoleChoice.OldSkool):
	                    InitializeOldSkoolShell();
	                    break;
	                case( ConsoleChoice.StudioShell):
	                    InitializeInternalConsole(resourceManager);
	                    break;
	                default:
                                    
	                    break;
	            }
	        }

	        if (null != _toolWindow)
	        {
	            _toolWindow.Visible = true;
	        }
	    }

	    private void InitializeInternalConsole(ResourceManager resourceManager)
	    {
	        object windowControlObj = null;
	        Assembly asm = typeof(PSTextBox).Assembly;
	        Windows2 windows = (Windows2) _applicationObject.Windows;
            if (null == _toolWindow)
            {
                _toolWindow = (Window2) windows.CreateToolWindow2(
                    _addInInstance,
                    asm.Location,
                    typeof (PSTextBox).FullName,
                    "StudioShell",
                    ToolWindowGuid,
                    ref windowControlObj);
                _consoleControl = (PSTextBox) windowControlObj;

                var image = resourceManager.GetObject("1") as Image;
                _toolWindow.SetTabPicture(ImageAdapter.ToIPictureDisp(image));
            }

            if (null != _toolWindow)
	        {               
                _toolWindow.Visible = true;

                //HACK: vs2010 uses a container window that does not automagically size the control window,
                //  so we have to force a WM_WINDOWPOSCHANGED message to set up the initial control dock state
	            _consoleControl.Height += 1;
	        }

            _consoleControl.ClearBuffer();

            if( null == Shell )
            {
                InitializeShell();
            }
	    }

	    [DllImport( "kernel32.dll")]
	    private static extern bool AllocConsole();

        void InitializeOldSkoolShell()
        {
            var thread = new System.Threading.Thread(() =>
                                                         {
                                                             var config = RunspaceConfiguration.Create();
                                                                                                                          
                                                             config.InitializationScripts.Append(new ScriptConfigurationEntry(
                                                                "warn-defaultconsole",
                                                                Scripts.WarnDefaultConsole
                                                                )
                                                                );

                                                             config.InitializationScripts.Append(new ScriptConfigurationEntry( 
                                                                 "start-studioshell",
                                                                 Scripts.StartStudioShell
                                                                 )
                                                                 );

                                                             config.InitializationScripts.Append(new ScriptConfigurationEntry(
                                                                 "start-profile",
                                                                 Scripts.CreateRunProfileScript(new StudioShellProfileInfo())
                                                                 )
                                                                 );

                                                             AllocConsole();
                                                             Microsoft.PowerShell.ConsoleShell.Start(
                                                                 config,
                                                                 "Visual Studio Default Process Console",
                                                                 "",                                                                 
                                                                 new string[] {});
                                                         });
            thread.Start();
        }

	    public static void ReloadAddins( object _dte )
	    {
	        EnvDTE.DTE dte = _dte as EnvDTE.DTE;
            dte.AddIns.Update();
	    }

	    public static void RegisterRunspace(Runspace runspace )
        {
            if( null == Shell )
            {
                Shell = new RunspaceCommandExecutor(runspace);
                Shell.Run();
            }
        }

        public static void PrepareCommandsForCustomHost()
        {
            var dte = Locator.Get<DTE2>();
            object[] contextGuids = new object[] { }; 
            var command = ((Commands2)dte.Commands).AddNamedCommand2(
                Connect.AddInInstance, "DoNothing",
                "Do Nothing", 
                "Does Nothing", 
                true,
                null,
                ref contextGuids,
                                (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                (int)vsCommandStyle.vsCommandStylePictAndText,
                vsCommandControlType.vsCommandControlTypeButton
                );

            try
            {
                dte.Commands.Raise(command.Guid, command.ID, null, null);
            }
            catch
            {                
            }

            try
            {
                command.Delete();
            }
            catch
            {                
            }
        }

	    void InitializeShell()
        {
            UISettings uiSettings = GetUISettings();
            _consoleControl.Apply(uiSettings);

	        var shellConfiguration = new StudioShellConfiguration(uiSettings);

            var shell = new Shell(_consoleControl, shellConfiguration);

            shell.ShouldExit += OnShellExit;
            shell.Progress += OnShellProgressReport;
            shell.CommandExecutionStateChange += OnShellCommandExecutionStateChange;
	        shell.CommandCancelTimeout += (s, a) => PromptForForceCommandCancel();

            Shell = shell;
            Locator.Set<Shell>( shell );

            Shell.Run();            
        }

	    private void OnShellCommandExecutionStateChange(object sender, CodeOwls.PowerShell.Host.Utility.EventArgs<bool> e)
	    {
	        var statusAnimation = _statusBarAnimation;
            if( null == statusAnimation )
            {
                return;
            }

	        statusAnimation.IsPSIconEnabled = e.Data;
	    }

	    private void OnShellProgressReport(object sender, ProgressRecordEventArgs e)
	    {
            if( (-1) != e.ProgressRecord.ParentActivityId)
            {
                return;
            }

            var isComplete = e.ProgressRecord.RecordType == ProgressRecordType.Completed;
	        var labelFormat = "{0}: {1}";
            if( ! String.IsNullOrEmpty( e.ProgressRecord.CurrentOperation ) )
            {
                labelFormat += " ({2})";
            }
            
            var label = String.Format(labelFormat, e.ProgressRecord.Activity, e.ProgressRecord.StatusDescription, e.ProgressRecord.CurrentOperation );

            var percent = isComplete
	                          ? 100
	                          : Math.Max( 0, e.ProgressRecord.PercentComplete);

            _applicationObject.StatusBar.Progress( ! isComplete, label, percent, 100 );            
	    }

	    private void OnShellExit(object sender, ExitEventArgs e)
	    {
	        CloseShell();
	    }

        void CloseShell()
        {
            try
            {
                _toolWindow.Visible = false;
            }
            catch
            {
                
            }
	    }

        void RestartShell()
        {
            try
            {
                Shell.Stop();
                //_toolWindow.Visible = false;
            }
            catch
            {
            }

            Shell.Dispose();
            Shell = null; 
            
            ExecuteStudioShellCommand();
        }

        ConsoleColor GetConsoleColor(uint ucolor, ConsoleColor defaultColor)
        {
            try
            {
                var bytes = BitConverter.GetBytes(ucolor);
                var color = Color.FromArgb(bytes[0], bytes[1], bytes[2]);
                return color.ToConsoleColor();
            }
            catch (Exception)
            {
            }
            return defaultColor;
        }

	    private UISettings GetUISettings()
	    {
	        UISettings settings = new UISettings();
            
	        var props = _applicationObject.get_Properties("FontsAndColors", "TextEditor");
	        settings.FontName = props.Item("FontFamily").Value.ToString();
	        settings.FontSize = Convert.ToInt32(props.Item("FontSize").Value.ToString());
	     
	        var colors = props.Item("FontsAndColorsItems").Object as FontsAndColorsItems;
            if (null == colors)
            {
                return settings;
            }
	        var item = colors.Item("Plain Text");
            settings.ForegroundColor = GetConsoleColor(item.Foreground, settings.ForegroundColor);
            settings.BackgroundColor = GetConsoleColor(item.Background, settings.BackgroundColor);

	        return settings;
	    }

	    private void ExtractUIColorSettingsFromFontsAndColors(FontsAndColorsItems colors, UISettings settings)
	    {
	        ColorableItems item;
	        item = colors.Item("Error");
	        if (null == item)
	        {
	            item = colors.Item("Compiler Error");
	        }
	        settings.ErrorForegroundColor = GetConsoleColor(item.Foreground, settings.ErrorForegroundColor);
	        settings.ErrorBackgroundColor = GetConsoleColor(item.Background, settings.ErrorBackgroundColor);

	        item = colors.Item("Warning");
	        if (null == item)
	        {
	            item = colors.Item("Compiler Warning");
	        }
	        settings.WarningForegroundColor = GetConsoleColor(item.Foreground, settings.WarningForegroundColor);
	        settings.WarningBackgroundColor = GetConsoleColor(item.Background, settings.WarningBackgroundColor);

	        item = colors.Item("String");
	        settings.DebugForegroundColor = GetConsoleColor(item.Foreground, settings.DebugForegroundColor);
	        settings.DebugBackgroundColor = GetConsoleColor(item.Background, settings.DebugBackgroundColor);

	        item = colors.Item("Comment");
	        settings.VerboseForegroundColor = GetConsoleColor(item.Foreground, settings.VerboseForegroundColor);
	        settings.VerboseBackgroundColor = GetConsoleColor(item.Background, settings.VerboseBackgroundColor);
	    }

	    private DTE2 _applicationObject;
		private AddIn _addInInstance;
	    private Window2 _toolWindow;
	    private PSTextBox _consoleControl;
	    private StatusBarAnimationState _statusBarAnimation;
	    private static Settings _settings;
        internal const string ToolWindowGuid = "{e9ce9b2a-88d1-48aa-843d-efded9cb8056}";
	    private const string StudioshellResources = "CodeOwls.StudioShell.Resources";

        private static IRunnableCommandExecutor Shell;
	    private string StudioShellCommandName = "CodeOwls.StudioShell.Connect.StudioShell";
        private string StudioShellRestartCommandName = "CodeOwls.StudioShell.Connect.ResetRunspace";
        private string StudioShellDoNothingCommandName = "CodeOwls.StudioShell.Connect.DoNothing";
	    private string StudioShellCancelCommandName = "CodeOwls.StudioShell.Connect.Cancel";
	    private string StudioShellExecuteCommandName = "CodeOwls.StudioShell.Connect.Execute";
	    public static DTE2 ApplicationObject { get; private set; }
 
        internal static AddIn AddInInstance{ get; private set; }
        internal static ICommandExecutor Executor { get { return Shell; } }
        
        Settings Settings 
        { 
            get
            {
                if( null == _settings )
                {
                    _settings = SettingsManager.Settings;
                }
                return _settings;
            }
        }
	}
}
