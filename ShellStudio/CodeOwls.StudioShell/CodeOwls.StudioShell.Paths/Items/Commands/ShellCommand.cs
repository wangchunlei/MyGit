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

namespace CodeOwls.StudioShell.Paths.Items.Commands
{
    public static class CommandExtensions
    {
        public static string GetSafeName( this Command command )
        {
            var name = command.Name;
            if( string.IsNullOrEmpty( name ))
            {
                name = "#" + command.ID;
            }
            return name;
        }
    }

    public class ShellCommand
    {
        private readonly Command _command;

        internal ShellCommand(Command command)
        {
            _command = command;
        }

        public string Name
        {
            get { return _command.Name; }
        }

        public string Guid
        {
            get { return _command.Guid; }
        }

        public int ID
        {
            get { return _command.ID; }
        }

        public bool IsAvailable
        {
            get { return _command.IsAvailable; }
        }

        public object Bindings
        {
            get { return _command.Bindings; }
            set { _command.Bindings = value; }
        }

        public string LocalizedName
        {
            get { return _command.LocalizedName; }
        }

        public object AddControl(object Owner, int Position)
        {
            return ShellObjectFactory.CreateFromCommandBarControl(_command.AddControl(Owner, Position));
        }

        public void Delete()
        {
            _command.Delete();
        }

        internal Command AsCommand()
        {
            return _command;
        }
    }
}