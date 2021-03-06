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
    public class ShellCodeVariable : ShellCodeModelElement2
    {
        private readonly CodeVariable2 _variable;

        internal ShellCodeVariable(CodeVariable2 variable) : base(variable as CodeElement2)
        {
            _variable = variable;
        }

        public object InitExpression
        {
            get { return _variable.InitExpression; }
            set { _variable.InitExpression = value; }
        }

        public ShellCodeTypeReference Type
        {
            get { return new ShellCodeTypeReference(_variable.Type as CodeTypeRef2); }
            set { _variable.Type = value.AsCodeTypeRef(); }
        }

        public vsCMAccess Access
        {
            get { return _variable.Access; }
            set { _variable.Access = value; }
        }

        public bool IsConstant
        {
            get { return _variable.IsConstant; }
            set { _variable.IsConstant = value; }
        }

        public IEnumerable<IShellCodeModelElement2> Attributes
        {
            get { return GetEnumerator(_variable.Attributes); }
        }

        public string DocComment
        {
            get { return _variable.DocComment; }
            set { _variable.DocComment = value; }
        }

        public string Comment
        {
            get { return _variable.Comment; }
            set { _variable.Comment = value; }
        }

        public bool IsShared
        {
            get { return _variable.IsShared; }
            set { _variable.IsShared = value; }
        }

        public vsCMConstKind ConstKind
        {
            get { return _variable.ConstKind; }
            set { _variable.ConstKind = value; }
        }

        public bool IsGeneric
        {
            get { return _variable.IsGeneric; }
        }

        public ShellCodeAttribute AddAttribute(string Name, string Value, object Position)
        {
            return new ShellCodeAttribute(_variable.AddAttribute(Name, Value, Position) as CodeAttribute2);
        }

        public string get_Prototype(int Flags)
        {
            return _variable.get_Prototype(Flags);
        }
    }
}