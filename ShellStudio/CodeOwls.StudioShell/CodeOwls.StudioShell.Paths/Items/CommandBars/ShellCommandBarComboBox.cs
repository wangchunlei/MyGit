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


using Microsoft.VisualStudio.CommandBars;

namespace CodeOwls.StudioShell.Paths.Items.CommandBars
{
    internal class ShellCommandBarComboBox
    {
        private readonly CommandBarComboBox _combo;

        internal ShellCommandBarComboBox(CommandBarComboBox combo)
        {
            _combo = combo;
        }

        public int Creator
        {
            get { return _combo.Creator; }
        }

        public bool BeginGroup
        {
            get { return _combo.BeginGroup; }
            set { _combo.BeginGroup = value; }
        }

        public bool BuiltIn
        {
            get { return _combo.BuiltIn; }
        }

        public string Caption
        {
            get { return _combo.Caption; }
            set { _combo.Caption = value; }
        }

        public string DescriptionText
        {
            get { return _combo.DescriptionText; }
            set { }
        }

        public bool Enabled
        {
            get { return _combo.Enabled; }
            set { _combo.Enabled = value; }
        }

        public int Height
        {
            get { return _combo.Height; }
            set { _combo.Height = value; }
        }

        public int Id
        {
            get { return _combo.Id; }
        }

        public int Index
        {
            get { return _combo.Index; }
        }

        public int InstanceId
        {
            get { return _combo.InstanceId; }
        }

        public ShellCommandBar Parent
        {
            get { return new ShellCommandBar(_combo.Parent); }
        }

        public string Parameter
        {
            get { return _combo.Parameter; }
            set { _combo.Parameter = value; }
        }

        public string Tag
        {
            get { return _combo.Tag; }
            set { _combo.Tag = value; }
        }

        public string TooltipText
        {
            get { return _combo.TooltipText; }
            set { _combo.TooltipText = value; }
        }

        public int Top
        {
            get { return _combo.Top; }
        }

        public MsoControlType Type
        {
            get { return _combo.Type; }
        }

        public bool Visible
        {
            get { return _combo.Visible; }
            set { _combo.Visible = value; }
        }

        public int Width
        {
            get { return _combo.Width; }
            set { _combo.Width = value; }
        }

        public int ListCount
        {
            get { return _combo.ListCount; }
        }

        public int ListHeaderCount
        {
            get { return _combo.ListHeaderCount; }
            set { _combo.ListHeaderCount = value; }
        }

        public int SelectedItemIndex
        {
            get { return _combo.ListIndex - 1; }
            set { _combo.ListIndex = value + 1; }
        }

        public MsoComboStyle Style
        {
            get { return _combo.Style; }
            set { _combo.Style = value; }
        }

        public void Reset()
        {
            _combo.Reset();
        }

        public void SetFocus()
        {
            _combo.SetFocus();
        }

        public ShellCommandBarComboBox Copy(object Bar, object Before)
        {
            return new ShellCommandBarComboBox((CommandBarComboBox) _combo.Copy(Bar, Before));
        }

        public void Delete(object Temporary)
        {
            _combo.Delete(Temporary);
        }

        public void Execute()
        {
            _combo.Execute();
        }

        public ShellCommandBarComboBox Move(object Bar, object Before)
        {
            return new ShellCommandBarComboBox((CommandBarComboBox) _combo.Move(Bar, Before));
        }

        public string get_List(int Index)
        {
            return _combo.get_List(Index);
        }

        public void set_List(int Index, string pbstrItem)
        {
            _combo.set_List(Index, pbstrItem);
        }

        public event _CommandBarComboBoxEvents_ChangeEventHandler Change
        {
            add { _combo.Change += value; }
            remove { _combo.Change -= value; }
        }
    }
}