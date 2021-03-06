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


using CodeOwls.StudioShell.Paths.Nodes.CodeModel;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Provider.Variables
{
    public class SelectedCodeElementVariable : DTEPSVariable
    {
        private readonly vsCMElement _codeElementType;

        public SelectedCodeElementVariable(DTE2 dte, string name, vsCMElement codeElementType ) : base(dte, name)
        {
            _codeElementType = codeElementType;
        }

        public override object Value
        {
            get
            {
                var factory = FileCodeModelNodeFactory.CreateNodeFactoryFromCurrentSelection(_dte, _codeElementType);
                if( null == factory)
                {
                    return null;
                }

                var value = factory.GetNodeValue();
                if( null == value )
                {
                    return null;
                }

                return value.Item;
            }
        }
    }
}
