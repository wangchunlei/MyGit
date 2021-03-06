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


using System.IO;

namespace CodeOwls.StudioShell.Paths.Items.Templates
{
    internal class ShellTemplate
    {
        private readonly string _language;
        private readonly string _name;

        internal ShellTemplate(string name, string language)
        {
            _name = name;
            _language = language;
        }

        internal ShellTemplate(DirectoryInfo info)
        {
            _language = info.FullName;
            _name = System.IO.Path.GetDirectoryName(_language);
        }

        internal ShellTemplate(FileInfo fileInfo)
        {
            _language = fileInfo.FullName;
            _name = System.IO.Path.GetFileNameWithoutExtension(_language);
        }

        public string Name
        {
            get { return _name; }
        }

        public string Language
        {
            get { return _language; }
        }
    }
}