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

using System.Collections.Generic;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.StudioShell.Paths.Items.UI;
using EnvDTE80;
using CodeOwls.PowerShell.Provider.PathNodes;
using Microsoft.VisualStudio.Shell;

namespace CodeOwls.StudioShell.Paths.Nodes.UI
{
    internal class ErrorListItemNodeFactory : NodeFactoryBase, IRemoveItem, IInvokeItem
    {
        private readonly ErrorItem _item;

        public ErrorListItemNodeFactory(ErrorItem item)
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
            return new PathNode(new ShellErrorListItem(_item), Name, false);
        }

        #endregion

        #region Implementation of IRemoveItem

        public object RemoveItemParameters
        {
            get { return null; }
        }

        public void RemoveItem(IContext context, string path, bool recurse)
        {
            var tasks = ErrorListNodeFactory.ErrorProvider.Tasks;

            foreach (ErrorTask task in tasks)
            {
                if (_item.Description == task.Text)
                {
                    tasks.Remove(task);
                    break;
                }
            }
        }

        #endregion

        #region Implementation of IInvokeItem

        public object InvokeItemParameters
        {
            get { return null; }
        }

        public IEnumerable<object> InvokeItem(IContext context, string path)
        {
            _item.Navigate();
            return null;
        }

        #endregion
    }
}