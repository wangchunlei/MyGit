
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


namespace CodeOwls.StudioShell.Configuration
{
    partial class SettingsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.radioButtonDefaultConsole = new System.Windows.Forms.RadioButton();
            this.radioButtonOldSkoolConsole = new System.Windows.Forms.RadioButton();
            this.textBoxVSCommand = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxLoadPowerShellProfiles = new System.Windows.Forms.CheckBox();
            this.checkBoxLoadStudioShellProfileScripts = new System.Windows.Forms.CheckBox();
            this.checkBoxLoadSolutionProfileScripts = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.restartNotice = new System.Windows.Forms.Label();
            this.checkBoxStartStudioShellWhenVsStarts = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonDefaultConsole
            // 
            this.radioButtonDefaultConsole.AutoSize = true;
            this.radioButtonDefaultConsole.Location = new System.Drawing.Point(28, 22);
            this.radioButtonDefaultConsole.Name = "radioButtonDefaultConsole";
            this.radioButtonDefaultConsole.Size = new System.Drawing.Size(59, 17);
            this.radioButtonDefaultConsole.TabIndex = 1;
            this.radioButtonDefaultConsole.TabStop = true;
            this.radioButtonDefaultConsole.Text = "Default";
            this.toolTip.SetToolTip(this.radioButtonDefaultConsole, "The default embedded console - functional, dockable, but subject to the UI thread" +
        "ing limitations in Visual Studio.");
            this.radioButtonDefaultConsole.UseVisualStyleBackColor = true;
            this.radioButtonDefaultConsole.CheckedChanged += new System.EventHandler(this.OnSelectedConsoleChanged);
            // 
            // radioButtonOldSkoolConsole
            // 
            this.radioButtonOldSkoolConsole.AutoSize = true;
            this.radioButtonOldSkoolConsole.Location = new System.Drawing.Point(28, 45);
            this.radioButtonOldSkoolConsole.Name = "radioButtonOldSkoolConsole";
            this.radioButtonOldSkoolConsole.Size = new System.Drawing.Size(77, 17);
            this.radioButtonOldSkoolConsole.TabIndex = 2;
            this.radioButtonOldSkoolConsole.TabStop = true;
            this.radioButtonOldSkoolConsole.Text = "Old School";
            this.toolTip.SetToolTip(this.radioButtonOldSkoolConsole, "A raw process console, far more responsive than the default, but not dockable.");
            this.radioButtonOldSkoolConsole.UseVisualStyleBackColor = true;
            this.radioButtonOldSkoolConsole.CheckedChanged += new System.EventHandler(this.OnSelectedConsoleChanged);
            // 
            // textBoxVSCommand
            // 
            this.textBoxVSCommand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxVSCommand.Location = new System.Drawing.Point(130, 91);
            this.textBoxVSCommand.Name = "textBoxVSCommand";
            this.textBoxVSCommand.Size = new System.Drawing.Size(266, 20);
            this.textBoxVSCommand.TabIndex = 5;
            this.textBoxVSCommand.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDefaultConsole);
            this.groupBox1.Controls.Add(this.textBoxVSCommand);
            this.groupBox1.Controls.Add(this.radioButtonOldSkoolConsole);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(385, 74);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Console Choice";
            // 
            // checkBoxLoadPowerShellProfiles
            // 
            this.checkBoxLoadPowerShellProfiles.AutoSize = true;
            this.checkBoxLoadPowerShellProfiles.Location = new System.Drawing.Point(28, 20);
            this.checkBoxLoadPowerShellProfiles.Name = "checkBoxLoadPowerShellProfiles";
            this.checkBoxLoadPowerShellProfiles.Size = new System.Drawing.Size(173, 17);
            this.checkBoxLoadPowerShellProfiles.TabIndex = 4;
            this.checkBoxLoadPowerShellProfiles.Text = "Load PowerShell Profile Scripts";
            this.toolTip.SetToolTip(this.checkBoxLoadPowerShellProfiles, "Enable this option to load your standard PowerShell console profile scripts when " +
        "StudioShell is initialized.");
            this.checkBoxLoadPowerShellProfiles.UseVisualStyleBackColor = true;
            // 
            // checkBoxLoadStudioShellProfileScripts
            // 
            this.checkBoxLoadStudioShellProfileScripts.AutoSize = true;
            this.checkBoxLoadStudioShellProfileScripts.Location = new System.Drawing.Point(28, 43);
            this.checkBoxLoadStudioShellProfileScripts.Name = "checkBoxLoadStudioShellProfileScripts";
            this.checkBoxLoadStudioShellProfileScripts.Size = new System.Drawing.Size(173, 17);
            this.checkBoxLoadStudioShellProfileScripts.TabIndex = 5;
            this.checkBoxLoadStudioShellProfileScripts.Text = "Load StudioShell Profile Scripts";
            this.toolTip.SetToolTip(this.checkBoxLoadStudioShellProfileScripts, "Enable this option to load StudioShell profile scripts when StudioShell is initia" +
        "lized.");
            this.checkBoxLoadStudioShellProfileScripts.UseVisualStyleBackColor = true;
            // 
            // checkBoxLoadSolutionProfileScripts
            // 
            this.checkBoxLoadSolutionProfileScripts.AutoSize = true;
            this.checkBoxLoadSolutionProfileScripts.Location = new System.Drawing.Point(28, 66);
            this.checkBoxLoadSolutionProfileScripts.Name = "checkBoxLoadSolutionProfileScripts";
            this.checkBoxLoadSolutionProfileScripts.Size = new System.Drawing.Size(134, 17);
            this.checkBoxLoadSolutionProfileScripts.TabIndex = 6;
            this.checkBoxLoadSolutionProfileScripts.Text = "Load Solution Modules";
            this.toolTip.SetToolTip(this.checkBoxLoadSolutionProfileScripts, "Enable this option to allow StudioShell to automatically locate and load Solution" +
        " Profile scripts.");
            this.checkBoxLoadSolutionProfileScripts.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxLoadSolutionProfileScripts);
            this.groupBox2.Controls.Add(this.checkBoxLoadStudioShellProfileScripts);
            this.groupBox2.Controls.Add(this.checkBoxLoadPowerShellProfiles);
            this.groupBox2.Location = new System.Drawing.Point(3, 83);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 93);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Profile Options";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxStartStudioShellWhenVsStarts);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.comboBox1);
            this.groupBox3.Location = new System.Drawing.Point(6, 182);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(382, 78);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Startup Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Logging Level:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "None",
            "Debug Only",
            "Verbose"});
            this.comboBox1.Location = new System.Drawing.Point(101, 46);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(229, 21);
            this.comboBox1.TabIndex = 8;
            // 
            // restartNotice
            // 
            this.restartNotice.AutoSize = true;
            this.restartNotice.Location = new System.Drawing.Point(28, 274);
            this.restartNotice.Name = "restartNotice";
            this.restartNotice.Size = new System.Drawing.Size(308, 13);
            this.restartNotice.TabIndex = 9;
            this.restartNotice.Text = "Note: Changes will not take effect until you restart Visual Studio.";
            // 
            // checkBoxStartStudioShellWhenVsStarts
            // 
            this.checkBoxStartStudioShellWhenVsStarts.AutoSize = true;
            this.checkBoxStartStudioShellWhenVsStarts.Location = new System.Drawing.Point(25, 23);
            this.checkBoxStartStudioShellWhenVsStarts.Name = "checkBoxStartStudioShellWhenVsStarts";
            this.checkBoxStartStudioShellWhenVsStarts.Size = new System.Drawing.Size(225, 17);
            this.checkBoxStartStudioShellWhenVsStarts.TabIndex = 7;
            this.checkBoxStartStudioShellWhenVsStarts.Text = "Run StudioShell when Visual Studio Starts";
            this.toolTip.SetToolTip(this.checkBoxStartStudioShellWhenVsStarts, "Enable this option to allow StudioShell to automatically locate and load Solution" +
        " Profile scripts.");
            this.checkBoxStartStudioShellWhenVsStarts.UseVisualStyleBackColor = true;
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.restartNotice);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(402, 307);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonDefaultConsole;
        private System.Windows.Forms.RadioButton radioButtonOldSkoolConsole;
        private System.Windows.Forms.TextBox textBoxVSCommand;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxLoadPowerShellProfiles;
        private System.Windows.Forms.CheckBox checkBoxLoadSolutionProfileScripts;
        private System.Windows.Forms.CheckBox checkBoxLoadStudioShellProfileScripts;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label restartNotice;
        private System.Windows.Forms.CheckBox checkBoxStartStudioShellWhenVsStarts;
    }
}
