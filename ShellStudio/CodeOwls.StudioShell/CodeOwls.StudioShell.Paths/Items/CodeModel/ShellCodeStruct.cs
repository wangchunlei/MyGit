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
    public class ShellCodeStruct : ShellCodeModelElement2
    {
        private readonly CodeStruct2 _struct;

        internal ShellCodeStruct(CodeStruct2 @struct) : base(@struct as CodeElement2)
        {
            _struct = @struct;
        }

        public ShellCodeNamespace Namespace
        {
            get { return new ShellCodeNamespace(_struct.Namespace); }
        }

        public IEnumerable<IShellCodeModelElement2> Bases
        {
            get { return GetEnumerator(_struct.Bases); }
        }

        public IEnumerable<IShellCodeModelElement2> Members
        {
            get { return GetEnumerator(_struct.Members); }
        }

        public vsCMAccess Access
        {
            get { return _struct.Access; }
            set { _struct.Access = value; }
        }

        public IEnumerable<IShellCodeModelElement2> Attributes
        {
            get { return GetEnumerator(_struct.Attributes); }
        }


        public string DocComment
        {
            get { return _struct.DocComment; }
            set { _struct.DocComment = value; }
        }

        public string Comment
        {
            get { return _struct.Comment; }
            set { _struct.Comment = value; }
        }

        public IEnumerable<IShellCodeModelElement2> DerivedTypes
        {
            get { return GetEnumerator(_struct.DerivedTypes); }
        }

        public IEnumerable<IShellCodeModelElement2> ImplementedInterfaces
        {
            get { return GetEnumerator(_struct.ImplementedInterfaces); }
        }

        public bool IsAbstract
        {
            get { return _struct.IsAbstract; }
            set { _struct.IsAbstract = value; }
        }

        public bool IsGeneric
        {
            get { return _struct.IsGeneric; }
        }

        public vsCMDataTypeKind DataTypeKind
        {
            get { return _struct.DataTypeKind; }
            set { _struct.DataTypeKind = value; }
        }

        public IEnumerable<IShellCodeModelElement2> Parts
        {
            get { return GetEnumerator(_struct.Parts); }
        }

        public IShellCodeModelElement2 AddBase(string Base, int Position)
        {
            CodeElement ce = _struct.AddBase(Base, Position);
            return ShellObjectFactory.CreateFromCodeElement(ce);
        }

        public ShellCodeAttribute AddAttribute(string Name, string Value, object Position)
        {
            return new ShellCodeAttribute(_struct.AddAttribute(Name, Value, Position) as CodeAttribute2);
        }

        public void RemoveBase(object Element)
        {
            _struct.RemoveBase(Element);
        }

        public void RemoveMember(object Element)
        {
            _struct.RemoveMember(Element);
        }

        public ShellCodeInterface AddImplementedInterface(object Base, object Position)
        {
            return new ShellCodeInterface(_struct.AddImplementedInterface(Base, Position) as CodeInterface2);
        }

        public ShellCodeMethod AddFunction(string Name, vsCMFunction Kind, object Type, object Position,
                                           vsCMAccess Access, object Location)
        {
            return
                new ShellCodeMethod(_struct.AddFunction(Name, Kind, Type, Position, Access, Location) as CodeFunction2);
        }

        public ShellCodeVariable AddVariable(string Name, object Type, object Position, vsCMAccess Access,
                                             object Location)
        {
            return new ShellCodeVariable(_struct.AddVariable(Name, Type, Position, Access, Location) as CodeVariable2);
        }

        public ShellCodeProperty AddProperty(string GetterName, string PutterName, object Type, object Position,
                                             vsCMAccess Access, object Location)
        {
            return new ShellCodeProperty(_struct.AddProperty(GetterName, PutterName, Type, Position, Access, Location));
        }

        public ShellCodeClass AddClass(string Name, object Position, object Bases, object ImplementedInterfaces,
                                       vsCMAccess Access)
        {
            return
                new ShellCodeClass(_struct.AddClass(Name, Position, Bases, ImplementedInterfaces, Access) as CodeClass2);
        }

        public ShellCodeStruct AddStruct(string Name, object Position, object Bases, object ImplementedInterfaces,
                                         vsCMAccess Access)
        {
            return
                new ShellCodeStruct(
                    _struct.AddStruct(Name, Position, Bases, ImplementedInterfaces, Access) as CodeStruct2);
        }

        public ShellCodeEnum AddEnum(string Name, object Position, object Bases, vsCMAccess Access)
        {
            return new ShellCodeEnum(_struct.AddEnum(Name, Position, Bases, Access));
        }

        public ShellCodeDelegate AddDelegate(string Name, object Type, object Position, vsCMAccess Access)
        {
            return new ShellCodeDelegate(_struct.AddDelegate(Name, Type, Position, Access) as CodeDelegate2);
        }

        public void RemoveInterface(object Element)
        {
            _struct.RemoveInterface(Element);
        }

        public ShellCodeEvent AddEvent(string Name, string FullDelegateName, bool CreatePropertyStyleEvent,
                                       object Location, vsCMAccess Access)
        {
            return
                new ShellCodeEvent(_struct.AddEvent(Name, FullDelegateName, CreatePropertyStyleEvent, Location, Access));
        }

        public bool get_IsDerivedFrom(string FullName)
        {
            return _struct.get_IsDerivedFrom(FullName);
        }
    }
}