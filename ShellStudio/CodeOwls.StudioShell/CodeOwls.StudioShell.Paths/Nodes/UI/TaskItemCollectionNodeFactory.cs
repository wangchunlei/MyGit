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
using CodeOwls.PowerShell.Provider.Attributes;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.StudioShell.Common.Utility;
using EnvDTE;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace CodeOwls.StudioShell.Paths.Nodes.UI
{
    [CmdletHelpPathID("TaskCollection")]
    public class TaskItemCollectionNodeFactory : CollectionNodeFactoryBase, INewItem
    {
        private readonly TaskItems _tasks;

        public TaskItemCollectionNodeFactory(TaskItems tasks)
        {
            _tasks = tasks;
        }

        #region Overrides of NodeFactoryBase

        public override string Name
        {
            get { return NodeNames.Tasks; }
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            return (
                       from TaskItem task in _tasks
                       select new TaskItemNodeFactory(task) as INodeFactory
                   ).ToList();
        }

        #endregion

        #region Implementation of INewItem

        public IEnumerable<string> NewItemTypeNames
        {
            get { return null; }
        }

        public object NewItemParameters
        {
            get { return new NewTaskItemParameters(); }
        }

        public IPathNode NewItem(IContext context, string path, string itemTypeName, object newItemValue)
        {
            if (path.StartsWith("$"))
            {
                throw new ArgumentException("Task item names cannot start with $");
            }
            if (path.ToLowerInvariant() == Name.ToLowerInvariant())
            {
                var items = GetNodeChildren( context );
                path = "$" + items.Count();
            }

            newItemValue = newItemValue ?? String.Empty;
            var p = (NewTaskItemParameters) context.DynamicParameters;

            var item = _tasks.Add(p.Category, p.Category, newItemValue.ToString(), p.Priority, p.Icon, p.Checkable, p.File,
                                  p.Line, p.ReadOnly, !p.NoFlush);
            item.Collection.ForceItemsToTaskList();

            var factory = new TaskItemNodeFactory(item);
            return factory.GetNodeValue();
        }

        #endregion

        #region Nested type: NewTaskItemParameters

        public class NewTaskItemParameters
        {
            public NewTaskItemParameters()
            {
                Category = "";
                Icon = vsTaskIcon.vsTaskIconUser;
                Priority = vsTaskPriority.vsTaskPriorityMedium;
                Checkable = false;
                File = String.Empty;
                Line = 0;
                ReadOnly = false;
                NoFlush = false;
            }

            [Parameter(HelpMessage = "The task category")]
            public string Category { get; set; }

            //[Parameter(Mandatory = true)]
            //public string Description { get; set; }
            [Parameter(HelpMessage = "The task priority")]
            public vsTaskPriority Priority { get; set; }

            [Parameter(HelpMessage = "The icon to show for the task")]
            public vsTaskIcon Icon { get; set; }

            [Parameter(HelpMessage = "Specifies whether the task displays a checkbox")]
            public SwitchParameter Checkable { get; set; }

            [Parameter(HelpMessage = "The file associated with this task", ValueFromPipelineByPropertyName = true)]
            public string File { get; set; }

            [Parameter(HelpMessage = "The file line index associated with this task")]
            public int Line { get; set; }

            [Parameter(HelpMessage = "Specifies that the task cannot be deleted by the user")]
            public SwitchParameter ReadOnly { get; set; }

            [Parameter(Mandatory = false,
                HelpMessage = "Indicates that the task should not be immediately flushed to the task pane")]
            public SwitchParameter NoFlush { get; set; }
        }

        #endregion
    }
}