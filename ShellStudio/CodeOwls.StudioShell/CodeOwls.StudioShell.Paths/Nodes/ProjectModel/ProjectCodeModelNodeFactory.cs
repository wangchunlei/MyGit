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
using EnvDTE;

namespace CodeOwls.StudioShell.Paths.Nodes.ProjectModel
{
    [CmdletHelpPathID("ProjectCodeModel")]
    public class ProjectCodeModelNodeFactory : ProjectNodeFactory
    {
        public ProjectCodeModelNodeFactory(Project project) : base(project)
        {
        }

        public override IEnumerable<INodeFactory> GetNodeChildren(IContext context)
        {
            foreach (ProjectItem item in _project.ProjectItems)
            {
                if (item is Project)
                {
                    yield return new ProjectCodeModelNodeFactory(item as Project);
                }

                var projectItem = item as ProjectItem;

                if (null != projectItem.SubProject)
                {
                    yield return new ProjectCodeModelNodeFactory(projectItem.SubProject);
                }

                if (null != item.FileCodeModel)
                {
                    yield return new ProjectItemCodeModelNodeFactory(item);
                }

                if (item.Kind == Constants.vsProjectItemKindPhysicalFolder)
                {
                    yield return new ProjectFolderCodeModelItemNodeFactory(item);
                }
            }
        }

    }
}
