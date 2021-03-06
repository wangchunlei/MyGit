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
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Items.CodeModel
{
    public class ShellCodeEvent : ShellCodeModelElement2
    {
        private readonly CodeEvent _event;

        internal ShellCodeEvent(CodeEvent @event) : base(@event as CodeElement2)
        {
            _event = @event;
        }

        public object Parent
        {
            get { return _event.Parent; }
        }

        public vsCMAccess Access
        {
            get { return _event.Access; }
            set { _event.Access = value; }
        }

        public IEnumerable<IShellCodeModelElement2> Attributes
        {
            get { return GetEnumerator(_event.Attributes); }
        }

        public string DocComment
        {
            get { return _event.DocComment; }
            set { _event.DocComment = value; }
        }

        public string Comment
        {
            get { return _event.Comment; }
            set { _event.Comment = value; }
        }

        public ShellCodeMethod Adder
        {
            get { return new ShellCodeMethod(_event.Adder as CodeFunction2); }
            set { _event.Adder = value.AsCodeFunction(); }
        }

        public ShellCodeMethod Remover
        {
            get { return new ShellCodeMethod(_event.Remover as CodeFunction2); }
            set { _event.Remover = value.AsCodeFunction(); }
        }

        public ShellCodeMethod Thrower
        {
            get { return new ShellCodeMethod(_event.Thrower as CodeFunction2); }
            set { _event.Thrower = value.AsCodeFunction(); }
        }

        public bool IsPropertyStyleEvent
        {
            get { return _event.IsPropertyStyleEvent; }
        }

        public ShellCodeTypeReference Type
        {
            get { return new ShellCodeTypeReference(_event.Type as CodeTypeRef2); }
            set { _event.Type = value.AsCodeTypeRef(); }
        }

        public vsCMOverrideKind OverrideKind
        {
            get { return _event.OverrideKind; }
            set { _event.OverrideKind = value; }
        }

        public bool IsShared
        {
            get { return _event.IsShared; }
            set { _event.IsShared = value; }
        }

        public ShellCodeAttribute AddAttribute(string Name, string Value, object Position)
        {
            return new ShellCodeAttribute(_event.AddAttribute(Name, Value, Position) as CodeAttribute2);
        }

        public string get_Prototype(int Flags)
        {
            return _event.get_Prototype(Flags);
        }
    }
}