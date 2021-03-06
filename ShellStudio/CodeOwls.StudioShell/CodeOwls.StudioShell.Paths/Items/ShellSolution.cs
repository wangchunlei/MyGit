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
using System.Linq;
using CodeOwls.StudioShell.Paths.Items.ProjectModel;
using CodeOwls.StudioShell.Paths.Items.PropertyModel;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Items
{
    public class ShellSolution
    {
        private readonly Solution2 _solution;

        internal ShellSolution(Solution2 solution)
        {
            _solution = solution;
        }

        public int Count
        {
            get { return _solution.Count; }
        }

        public string FileName
        {
            get { return _solution.FileName; }
        }

        public IEnumerable<ShellProperty> Properties
        {
            get
            {
                return from Property prop in _solution.Properties
                       select new ShellProperty(prop);
            }
        }

        public bool IsDirty
        {
            get { return _solution.IsDirty; }
            set { _solution.IsDirty = value; }
        }

        public string FullName
        {
            get { return _solution.FullName; }
        }

        public bool Saved
        {
            get { return _solution.Saved; }
            set { _solution.Saved = value; }
        }

        public Globals Globals
        {
            get { return _solution.Globals; }
        }

        public object AddIns
        {
            get { return _solution.AddIns; }
        }

        public bool IsOpen
        {
            get { return _solution.IsOpen; }
        }

        public SolutionBuild SolutionBuild
        {
            get { return _solution.SolutionBuild; }
        }

        public IEnumerable<ShellProject> Projects
        {
            get
            {
                return from Project proj in _solution.Projects
                       select new ShellProject(proj);
            }
        }

        public ShellProject Item(object index)
        {
            return new ShellProject(_solution.Item(index));
        }

        public void SaveAs(string FileName)
        {
            _solution.SaveAs(FileName);
        }

        public ShellProject AddFromTemplate(string FileName, string Destination, string ProjectName, bool Exclusive)
        {
            return new ShellProject(_solution.AddFromTemplate(FileName, Destination, ProjectName, Exclusive));
        }

        public ShellProject AddFromFile(string FileName, bool Exclusive)
        {
            return new ShellProject(_solution.AddFromFile(FileName, Exclusive));
        }

        public void Open(string FileName)
        {
            _solution.Open(FileName);
        }

        public void Close(bool SaveFirst)
        {
            _solution.Close(SaveFirst);
        }

        public void Remove(Project proj)
        {
            _solution.Remove(proj);
        }

        public void Create(string Destination, string Name)
        {
            _solution.Create(Destination, Name);
        }

        public ShellProjectItem FindProjectItem(string FileName)
        {
            return new ShellProjectItem(_solution.FindProjectItem(FileName));
        }

        public string ProjectItemsTemplatePath(string ProjectKind)
        {
            return _solution.ProjectItemsTemplatePath(ProjectKind);
        }

        public ShellProject AddSolutionFolder(string Name)
        {
            return new ShellProject(_solution.AddSolutionFolder(Name));
        }

        public string GetProjectTemplate(string TemplateName, string Language)
        {
            return _solution.GetProjectTemplate(TemplateName, Language);
        }

        public string GetProjectItemTemplate(string TemplateName, string Language)
        {
            return _solution.GetProjectItemTemplate(TemplateName, Language);
        }

        public string get_TemplatePath(string ProjectType)
        {
            return _solution.get_TemplatePath(ProjectType);
        }
    }
}