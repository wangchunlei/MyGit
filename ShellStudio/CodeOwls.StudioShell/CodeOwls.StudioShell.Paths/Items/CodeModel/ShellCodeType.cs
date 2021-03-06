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
    public class ShellCodeType : ShellCodeModelElement2
    {
        private readonly CodeType _type;

        internal ShellCodeType(CodeType type) : base(type as CodeElement)
        {
            _type = type;
        }

        public ShellCodeNamespace Namespace
        {
            get { return new ShellCodeNamespace(_type.Namespace); }
        }

        public IEnumerable<IShellCodeModelElement2> Bases
        {
            get { return GetEnumerator(_type.Bases); }
        }

        public IEnumerable<IShellCodeModelElement2> Members
        {
            get { return GetEnumerator(_type.Members); }
        }


        public vsCMAccess Access
        {
            get { return _type.Access; }
            set { _type.Access = value; }
        }

        public IEnumerable<IShellCodeModelElement2> Attributes
        {
            get { return GetEnumerator(_type.Attributes); }
        }


        public string DocComment
        {
            get { return _type.DocComment; }
            set { _type.DocComment = value; }
        }

        public string Comment
        {
            get { return _type.Comment; }
            set { _type.Comment = value; }
        }

        public IEnumerable<IShellCodeModelElement2> DerivedTypes
        {
            get { return GetEnumerator(_type.DerivedTypes); }
        }

        public IShellCodeModelElement2 AddBase(string Base, int Position)
        {
            CodeElement ce = _type.AddBase(Base, Position);
            return ShellObjectFactory.CreateFromCodeElement(ce);
        }

        public ShellCodeAttribute AddAttribute(string Name, string Value, object Position)
        {
            return new ShellCodeAttribute(_type.AddAttribute(Name, Value, Position) as CodeAttribute2);
        }

        public void RemoveBase(object Element)
        {
            _type.RemoveBase(Element);
        }

        public void RemoveMember(object Element)
        {
            _type.RemoveMember(Element);
        }

        public bool get_IsDerivedFrom(string FullName)
        {
            return _type.get_IsDerivedFrom(FullName);
        }

        internal CodeType AsCodeType()
        {
            return _type;
        }
    }
}