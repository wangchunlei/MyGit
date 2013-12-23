
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CodeOwls.StudioShell.Common.Configuration;
using EnvDTE;
using CodeOwls.StudioShell.Utility;

namespace CodeOwls.StudioShell.Configuration
{
    public partial class SettingsControl : UserControl, IDTToolsOptionsPage
    {
        private Settings _settings;

        public SettingsControl()
        {
            InitializeComponent();
        }

        void LoadSettings()
        {
            _settings = SettingsManager.Settings;
        }

        #region Implementation of IDTToolsOptionsPage

        public void OnAfterCreated(EnvDTE.DTE DTEObject)
        {
            LoadSettings();
            ApplySettings();
        }

        private void ApplySettings()
        {
            var map = new Dictionary<ConsoleChoice, RadioButton>
                          {
                              {ConsoleChoice.OldSkool, radioButtonOldSkoolConsole},
                              {ConsoleChoice.StudioShell, radioButtonDefaultConsole}
                          };

            map[_settings.ConsoleChoice].Checked = true;

            checkBoxLoadPowerShellProfiles.Checked = _settings.LoadPowerShellProfiles;
            checkBoxLoadStudioShellProfileScripts.Checked = _settings.LoadStudioShellProfiles;
            checkBoxLoadSolutionProfileScripts.Checked = _settings.AutoManagePerSolutionModules;
            checkBoxStartStudioShellWhenVsStarts.Checked = _settings.RunStudioShellOnStartup;

            comboBox1.SelectedIndex = (int) _settings.StartupLogLevel;

            restartNotice.Visible = null != Connect.Executor;
        }

        public void GetProperties(ref object PropertiesObject)
        {
            if( null == _settings )
            {
                LoadSettings();
            }

            PropertiesObject = _settings;
        }

        public void OnOK()
        {
            _settings.ConsoleChoice = radioButtonOldSkoolConsole.Checked
                                                ? ConsoleChoice.OldSkool
                                                : ConsoleChoice.StudioShell;

            _settings.LoadPowerShellProfiles = checkBoxLoadPowerShellProfiles.Checked;
            _settings.LoadStudioShellProfiles = checkBoxLoadStudioShellProfileScripts.Checked;
            _settings.AutoManagePerSolutionModules = checkBoxLoadSolutionProfileScripts.Checked;
            _settings.RunStudioShellOnStartup = checkBoxStartStudioShellWhenVsStarts.Checked;

            _settings.StartupLogLevel = StartupLogLevel.None;

            var logValue = comboBox1.SelectedItem.ToString().ToLowerInvariant();
            if( logValue.Contains( "debug"))
            {
                _settings.StartupLogLevel = StartupLogLevel.Debug;
            }
            else if( logValue.Contains("verbose"))
            {
                _settings.StartupLogLevel = StartupLogLevel.Verbose;
            }
            
            SettingsManager.Save( _settings );
        }

        public void OnCancel()
        {
        }

        public void OnHelp()
        {
        }

        #endregion

        private void OnSelectedConsoleChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = ( radioButtonDefaultConsole.Checked || radioButtonOldSkoolConsole.Checked );
        }

    }
   
}
