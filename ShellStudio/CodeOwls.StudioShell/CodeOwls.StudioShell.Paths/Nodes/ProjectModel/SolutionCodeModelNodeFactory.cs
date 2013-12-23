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
using CodeOwls.PowerShell.Provider.Attributes;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Common.Utility;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Nodes.ProjectModel
{
    [CmdletHelpPathID("SolutionCodeModel")]
    public class SolutionCodeModelNodeFactory : SolutionProjectsNodeFactory
    {
        public SolutionCodeModelNodeFactory(DTE2 dte) : base( dte )
        {
        }

        public override string Name
        {
            get { return NodeNames.CodeModel; }
        }

        public override IEnumerable<INodeFactory> GetNodeChildren(IContext context)
        {
            var factories = new List<INodeFactory>();
            foreach (Project project in _dte.Solution.Projects)
            {
                factories.Add(new ProjectCodeModelNodeFactory(project));
            }
            return factories;
        }
    }
}
