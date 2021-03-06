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

namespace CodeOwls.StudioShell.Paths.Items.AddIns
{
    public class ShellAddIn
    {
        private readonly AddIn _addIn;

        public ShellAddIn(AddIn addIn)
        {
            _addIn = addIn;
        }

        public string Description
        {
            get { return _addIn.Description; }
            set { _addIn.Description = value; }
        }

        public string ProgID
        {
            get { return _addIn.ProgID; }
        }

        public string Guid
        {
            get { return _addIn.Guid; }
        }

        public bool Connected
        {
            get { return _addIn.Connected; }
            set { _addIn.Connected = value; }
        }

        //public object Instance
        //{
        //    get { return _addIn.Object; }
        //    set { _addIn.Object = value; }
        //}

        public string Name
        {
            get { return _addIn.Name; }
        }

        public string SatelliteDllPath
        {
            get { return _addIn.SatelliteDllPath; }
        }

        public void Remove()
        {
            _addIn.Remove();
        }
    }
}