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
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Items.CodeModel
{
    public class ShellCodeProperty2 : ShellCodeProperty
    {
        private readonly CodeProperty2 _property;

        internal ShellCodeProperty2(CodeProperty2 property) : base(property)
        {
            _property = property;
        }

        public object Parent
        {
            get { return (object)_property.Parent ?? (object)_property.Parent2; }
        }
        
        public IEnumerable<IShellCodeModelElement2> Parameters
        {
            get { return GetEnumerator(_property.Parameters); }
        }

        public bool IsGeneric
        {
            get { return _property.IsGeneric; }
        }

        public vsCMOverrideKind OverrideKind
        {
            get { return _property.OverrideKind; }
            set { _property.OverrideKind = value; }
        }

        public bool IsShared
        {
            get { return _property.IsShared; }
            set { _property.IsShared = value; }
        }

        public bool IsDefault
        {
            get { return _property.IsDefault; }
            set { _property.IsDefault = value; }
        }

        public bool CanGet
        {
            get
            {
                return (ReadWrite & vsCMPropertyKind.vsCMPropertyKindWriteOnly) !=
                       vsCMPropertyKind.vsCMPropertyKindWriteOnly;
            }
        }

        public bool CanSet
        {
            get
            {
                return (ReadWrite & vsCMPropertyKind.vsCMPropertyKindReadOnly) !=
                       vsCMPropertyKind.vsCMPropertyKindReadOnly;
            }
        }

        public vsCMPropertyKind ReadWrite
        {
            get { return _property.ReadWrite; }
        }

        public ShellCodeParameter AddParameter(string Name, object Type, object Position)
        {
            return new ShellCodeParameter(_property.AddParameter(Name, Type, Position) as CodeParameter2);
        }

        public void RemoveParameter(object Element)
        {
            _property.RemoveParameter(Element);
        }
    }
}