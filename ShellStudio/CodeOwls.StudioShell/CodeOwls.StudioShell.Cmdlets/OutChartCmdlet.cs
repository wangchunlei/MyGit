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
using System.Windows.Forms;
using CodeOwls.StudioShell.DataPaneControls;

namespace CodeOwls.StudioShell.Cmdlets
{
    [Cmdlet(VerbsData.Out, "chart")]
    public class OutChartCmdlet : OutSubsetDataPaneCmdletBase
    {
        [Parameter]
        [Alias("SeriesType", "Type")]
        public string ChartType { get; set; }

        [Parameter( Mandatory = true )]
        [Alias("X","Label","Group","Category")]
        public string GroupAxis { get; set; }

        protected override void BeginProcessing()
        {
            if (!String.IsNullOrEmpty(GroupAxis) &&
                !Include.Contains(GroupAxis, StringComparer.InvariantCultureIgnoreCase))
            {
                var names = new List<string>(Include);
                names.Add(GroupAxis);
                Include = names.ToArray();
            }
            base.BeginProcessing();
        }

        protected override Control GetPaneControl()
        {
            var control = new DataChartControl();
            control.SetDataSource(_data, ChartType, GroupAxis);

            return control;
        }
    }
}
