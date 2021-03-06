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


using EnvDTE;
using VSLangProj;
using VSLangProj80;

namespace CodeOwls.StudioShell.Paths.Items.ProjectModel
{
    public class ShellReference
    {
        private readonly Reference _reference;

        internal ShellReference(Reference reference)
        {
            _reference = reference;
        }

        public bool SpecificVersion
        {
            get
            {
                Reference3 r = _reference as Reference3;
                if (null == r)
                {
                    return false;
                }
                return r.SpecificVersion;
            }
            set
            {
                Reference3 r = _reference as Reference3;
                if (null == r)
                {
                    return;
                }
                r.SpecificVersion = value;
            }
        }

        public ShellProject ContainingProject
        {
            get { return new ShellProject(_reference.ContainingProject); }
        }

        public string Name
        {
            get { return _reference.Name; }
        }

        public prjReferenceType Type
        {
            get { return _reference.Type; }
        }

        public string Identity
        {
            get { return _reference.Identity; }
        }

        public string Path
        {
            get { return _reference.Path; }
        }

        public string Description
        {
            get { return _reference.Description; }
        }

        public string Culture
        {
            get { return _reference.Culture; }
        }

        public int MajorVersion
        {
            get { return _reference.MajorVersion; }
        }

        public int MinorVersion
        {
            get { return _reference.MinorVersion; }
        }

        public int RevisionNumber
        {
            get { return _reference.RevisionNumber; }
        }

        public int BuildNumber
        {
            get { return _reference.BuildNumber; }
        }

        public bool StrongName
        {
            get { return _reference.StrongName; }
        }

        public Project SourceProject
        {
            get { return _reference.SourceProject; }
        }

        public bool CopyLocal
        {
            get { return _reference.CopyLocal; }
            set { _reference.CopyLocal = value; }
        }

        public string PublicKeyToken
        {
            get { return _reference.PublicKeyToken; }
        }

        public string Version
        {
            get { return _reference.Version; }
        }

        public void Remove()
        {
            _reference.Remove();
        }
    }
}