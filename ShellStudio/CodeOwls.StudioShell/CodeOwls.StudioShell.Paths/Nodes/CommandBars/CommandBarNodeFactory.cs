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
using System.Linq;
using System.Management.Automation;
using CodeOwls.PowerShell.Paths.Extensions;
using CodeOwls.PowerShell.Provider.Attributes;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Common;
using CodeOwls.StudioShell.Common.Utility;
using CodeOwls.StudioShell.Paths.Items;
using CodeOwls.StudioShell.Paths.Items.CommandBars;
using CodeOwls.StudioShell.Paths.Items.Commands;
using CodeOwls.StudioShell.Paths.Utility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace CodeOwls.StudioShell.Paths.Nodes.CommandBars
{
    public abstract class CommandBarControlNodeFactory : NodeFactoryBase, IRemoveItem

    {
        private readonly CommandBarControl _control;
        private readonly bool _isContainer;

        protected CommandBarControlNodeFactory(CommandBarControl control, bool isContainer)
        {
            _control = control;
            _isContainer = isContainer;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return _control.Caption.MakeSafeForPath(); }
        }

        public override IPathNode GetNodeValue()
        {
            return new PathNode(ShellObjectFactory.CreateFromCommandBarControl(_control), Name, _isContainer);
        }

        #endregion

        internal static CommandBarControlNodeFactory Create(CommandBarControl control)
        {
            if (control is CommandBarPopup)
            {
                return new CommandBarPopupNodeFactory(control as CommandBarPopup);
            }
            if (control is CommandBarComboBox)
            {
                return new CommandBarComboBoxNodeFactory(control as CommandBarComboBox);
            }
            if (control is CommandBarButton)
            {
                return new CommandBarButtonNodeFactory(control as CommandBarButton);
            }

            return null;
        }

        #region Implementation of IRemoveItem

        public object RemoveItemParameters
        {
            get { return null; }
        }

        public void RemoveItem(IContext context, string path, bool recurse)
        {
            _control.Delete(false);
        }

        #endregion
       
    }

    public class CommandBarButtonNodeFactory : CommandBarControlNodeFactory, IInvokeItem
    {
        private readonly CommandBarButton _button;

        public CommandBarButtonNodeFactory(CommandBarButton button) : base(button, false)
        {
            _button = button;
        }

        public object InvokeItemParameters { get { return null; } }
        public IEnumerable<object> InvokeItem(IContext context, string path)
        {
            _button.Execute();
            return null;
        }
    }

    public class CommandBarComboBoxNodeFactory : CommandBarControlNodeFactory
    {
        private readonly CommandBarComboBox _comboBox;

        public CommandBarComboBoxNodeFactory(CommandBarComboBox comboBox)
            : base(comboBox, false)
        {
            _comboBox = comboBox;
        }
    }

    [CmdletHelpPathID("CommandBarControl")]
    public class CommandBarPopupNodeFactory : CommandBarControlNodeFactory, INewItem
    {
        private readonly CommandBarPopup _popup;

        public CommandBarPopupNodeFactory(CommandBarPopup popup) : base(popup, true)
        {
            _popup = popup;
        }

        #region Overrides of NodeFactoryBase

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            List<INodeFactory> factories = new List<INodeFactory>();

            foreach (CommandBarControl control in _popup.Controls)
            {
                factories.Add(CommandBarControlNodeFactory.Create(control));
            }

            return factories;
        }

        #endregion

        #region Implementation of INewItem

        private const string ButtonTypeName = "button";
        private const string PopupTypeName = "popup";

        public IEnumerable<string> NewItemTypeNames
        {
            get { return new[] {ButtonTypeName, PopupTypeName}; }
        }

        public object NewItemParameters
        {
            get { return new NewItemDynamicParameters(); }
        }

        public IPathNode NewItem(IContext context, string path, string itemTypeName, object newItemValue)
        {
            itemTypeName = itemTypeName ?? ButtonTypeName;

            var p = context.DynamicParameters as NewItemDynamicParameters;
            string caption = p.Caption ?? path;

            CommandBarControl ctrl = null;
            switch (itemTypeName.ToLowerInvariant())
            {
                case( PopupTypeName ):
                    {
                        object id = p.Id != 0 ? p.Id : Type.Missing;
                        object param = p.Parameter ?? Type.Missing;
                        object index = p.Index != 0 ? p.Index : Type.Missing;

                        ctrl = NewPopup(context, caption, id, param, index);
                        break;
                    }
                case( ButtonTypeName):
                default:
                    {
                        ctrl = NewButton(context, caption, p.Index, p.Binding, newItemValue);
                        break;
                    }
            }
            
            return CommandBarControlNodeFactory.Create(ctrl).GetNodeValue();
        }

        private CommandBarControl NewPopup(IContext context, string caption, object id, object o, object index)
        {
            var ctrl = _popup.Controls.Add(MsoControlType.msoControlPopup, id, o, index, Type.Missing);
            ctrl.Caption = caption;
            return ctrl;
        }

        public CommandBarControl NewButton(IContext context, string caption, int index, string binding, object newItemValue)
        {
            var validValueTypes = new[] {typeof (ScriptBlock), typeof (string)};
            if (null == newItemValue || !validValueTypes.Contains(newItemValue.GetType()))
            {
                var validNames = String.Join(", ", validValueTypes.ToList().ConvertAll(t => t.FullName).ToArray());
                throw new ArgumentException(
                    "new item values for command bar buttons must be one of the following types: " + validNames);
            }

            index = Math.Max(index, 1);
            ShellCommand shellCommand = CommandUtilities.GetOrCreateCommand(context, _popup.Application as DTE2, caption,
                                                                            newItemValue);
            if ( !String.IsNullOrEmpty(binding))
            {
                shellCommand.Bindings = new[] { (object)binding };
            }
            
            Command command = shellCommand.AsCommand();
            string n = command.Name;
            var cid = command.ID;
            var guid = command.Guid;
            
            var ctl = command.AddControl(_popup.CommandBar, index) as CommandBarControl;
            return ctl;
        }

        #endregion

        #region Nested type: NewItemDynamicParameters

        public class NewItemDynamicParameters
        {
            [Parameter]
            public int Id { get; set; }

            [Parameter]
            [Alias("Position")]
            public int Index { get; set; }

            [Parameter]
            public string Parameter { get; set; }

            [Parameter]
            public string Caption { get; set; }

            [Parameter]
            public string Binding { get; set; }
        }

        #endregion
    }

    [CmdletHelpPathID("CommandBarControl")]
    [CmdletHelpPathID("CommandBar")]
    public class CommandBarNodeFactory : NodeFactoryBase, IRemoveItem, INewItem
    {
        private readonly CommandBar _commandBar;

        public CommandBarNodeFactory(CommandBar commandBar)
        {
            _commandBar = commandBar;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return _commandBar.Name.MakeSafeForPath(); }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            List<INodeFactory> factories = new List<INodeFactory>();

            foreach (CommandBarControl control in _commandBar.Controls)
            {
                factories.Add(CommandBarControlNodeFactory.Create(control));
            }

            return factories;
        }

        public override IPathNode GetNodeValue()
        {
            return new PathNode(new ShellCommandBar(_commandBar), Name, true);
        }

        #endregion

        #region Implementation of IRemoveItem

        public object RemoveItemParameters
        {
            get { return null; }
        }

        public void RemoveItem(IContext context, string path, bool recurse)
        {
            _commandBar.Delete();
        }

        #endregion

        #region Implementation of INewItem

        private const string ButtonTypeName = "button";
        private const string PopupTypeName = "popup";

        public IEnumerable<string> NewItemTypeNames
        {
            get { return new[] {ButtonTypeName, PopupTypeName}; }
        }

        public object NewItemParameters
        {
            get { return new NewItemDynamicParameters(); }
        }


        public IPathNode NewItem(IContext context, string path, string itemTypeName, object newItemValue)
        {
            itemTypeName = itemTypeName ?? ButtonTypeName;

            var p = context.DynamicParameters as NewItemDynamicParameters;
            string caption = p.Caption ?? path;

            CommandBarControl ctl = null;
            switch (itemTypeName.ToLowerInvariant())
            {
                case (PopupTypeName):
                    {
                        object id = p.Id != 0 ? p.Id : Type.Missing;
                        object param = p.Parameter ?? Type.Missing;
                        object index = p.Index != 0 ? p.Index : Type.Missing;
            
                        ctl = NewPopup(context, caption, id, param, index);
                        break;
                    }
                case (ButtonTypeName):
                default:
                    {
                        ctl = NewButton(context, caption, p.Index, p.Binding, newItemValue);
                        break;
                    }
            }

            return CommandBarControlNodeFactory.Create(ctl).GetNodeValue();
        }
/*
        public IPathNode NewButton(IContext context, string path, string itemTypeName, object newItemValue)
        {
            var validValueTypes = new[] {typeof (ScriptBlock), typeof (string)};
            if (null == newItemValue || !validValueTypes.Contains(newItemValue.GetType()))
            {
                var validNames = String.Join(", ", validValueTypes.ToList().ConvertAll(t => t.FullName).ToArray());
                throw new ArgumentException(
                    "new item values for command bar buttons must be one of the following types: " + validNames);
            }

            var p = context.DynamicParameters as NewItemDynamicParameters;
            MsoControlType type = MsoControlType.msoControlButton;
            object id = p.Id != 0 ? p.Id : Type.Missing;
            object param = p.Parameter ?? Type.Missing;
            int index = Math.Max(p.Index, 1);
            string caption = p.Caption ?? path;

            ShellCommand shellCommand = CommandUtilities.GetOrCreateCommand(
                context,
                _commandBar.Application as DTE2,
                caption,
                newItemValue
                );

            if (null != p.Binding)
            {
                shellCommand.Bindings = new[] {(object) p.Binding};
            }
            
            Command command = shellCommand.AsCommand();
            var ctl = command.AddControl(_commandBar, index) as CommandBarControl;
            return CommandBarControlNodeFactory.Create(ctl).GetNodeValue();
        }
*/


        private CommandBarControl NewPopup(IContext context, string caption, object id, object o, object index)
        {
            var ctrl = _commandBar.Controls.Add(MsoControlType.msoControlPopup, id, o, index, Type.Missing);
            ctrl.Caption = caption;
            return ctrl;
        }

        public CommandBarControl NewButton(IContext context, string caption, int index, string binding, object newItemValue)
        {
            var validValueTypes = new[] { typeof(ScriptBlock), typeof(string) };
            if (null == newItemValue || !validValueTypes.Contains(newItemValue.GetType()))
            {
                var validNames = String.Join(", ", validValueTypes.ToList().ConvertAll(t => t.FullName).ToArray());
                throw new ArgumentException(
                    "new item values for command bar buttons must be one of the following types: " + validNames);
            }

            index = Math.Max(index, 1);
            ShellCommand shellCommand = CommandUtilities.GetOrCreateCommand(
                context, 
                _commandBar.Application as DTE2, 
                caption,
                newItemValue
            );
            
            if (!String.IsNullOrEmpty(binding))
            {
                shellCommand.Bindings = new[] { (object)binding };
            }
            
            Command command = shellCommand.AsCommand();
            var ctl = command.AddControl(_commandBar, index) as CommandBarControl;
            return ctl;
        }

        public class NewItemDynamicParameters
        {
            [Parameter]
            public int Id { get; set; }

            [Parameter]
            [Alias("Position")]
            public int Index { get; set; }

            [Parameter]
            public string Parameter { get; set; }

            [Parameter]
            public string Caption { get; set; }

            [Parameter]
            public string Binding { get; set; }
        }

        #endregion
    }
}