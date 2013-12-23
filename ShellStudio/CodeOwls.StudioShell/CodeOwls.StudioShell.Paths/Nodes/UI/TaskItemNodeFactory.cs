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
using CodeOwls.StudioShell.Paths.Items.UI;
using EnvDTE;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace CodeOwls.StudioShell.Paths.Nodes.UI
{
    internal class TaskItemNodeFactory : NodeFactoryBase, IRemoveItem, IClearItem, ISetItem
    {
        private readonly TaskItem _item;

        public TaskItemNodeFactory(TaskItem item)
        {
            _item = item;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return _item.GetHashCode().ToString(); }
        }

        public override IPathNode GetNodeValue()
        {
            return new PathNode(new ShellTaskItem(_item), Name, false);
        }

        #endregion

        #region Implementation of IRemoveItem

        public object RemoveItemParameters
        {
            get { return null; }
        }

        public void RemoveItem(IContext context, string path, bool recurse)
        {
            _item.Delete();
        }

        #endregion

        #region Implementation of IClearItem

        public object ClearItemDynamicParamters
        {
            get { return null; }
        }

        public void ClearItem(IContext context, string path)
        {
            _item.Description = String.Empty;
        }

        #endregion

        #region Implementation of ISetItem

        public object SetItemParameters
        {
            get { return null; }
        }

        public IPathNode SetItem(IContext context, string path, object value)
        {
            if (value is String)
            {
                _item.Description = value as String;
            }
            else
            {
                var pso = PSObject.AsPSObject(value);
                var psothis = PSObject.AsPSObject(_item);

                foreach (var p in pso.Properties)
                {
                    if (! p.IsGettable)
                    {
                        continue;
                    }
                    var prop = psothis.Properties.Match(p.Name, PSMemberTypes.Properties).FirstOrDefault();
                    if (null == prop || ! prop.IsSettable)
                    {
                        continue;
                    }

                    prop.Value = p.Value;
                }
            }

            _item.Collection.ForceItemsToTaskList();
            return GetNodeValue();
        }

        #endregion
    }
}