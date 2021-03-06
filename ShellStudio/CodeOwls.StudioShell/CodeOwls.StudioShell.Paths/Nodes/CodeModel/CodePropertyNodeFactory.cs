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
using CodeOwls.PowerShell.Provider.Attributes;
using CodeOwls.StudioShell.Common.Exceptions;
using CodeOwls.StudioShell.Paths.Utility;
using EnvDTE;
using EnvDTE80;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace CodeOwls.StudioShell.Paths.Nodes.CodeModel
{
    [CmdletHelpPathID("CodeMethod")]
    public class CodePropertyNodeFactory : CodeElementWithChildrenNodeFactory
    {
        private readonly CodeProperty _property;

        public CodePropertyNodeFactory(CodeProperty property) : base(property as CodeElement)
        {
            _property = property;
        }

        protected override string CodeItemTypeName
        {
            get { return CodeItemTypes.Property; }
        }

        public override IEnumerable<string> NewItemTypeNames
        {
            get { return CodeItemTypes.PropertyNewItemTypeNames; }
        }

        protected override object NewAttribute(NewCodeElementItemParams p, string path, object newItemValue)
        {
            var value = null == newItemValue ? null : newItemValue.ToString();
            return _property.AddAttribute(path,
                                          value,
                                          p.Position.ToDTEParameter());
        }

        protected override object NewParameter(NewCodeElementItemParams newItemParams, string path, object value)
        {
            CodeProperty2 p2 = _property as CodeProperty2;
            if (null == p2)
            {
                throw new PropertyDoesNotSupportParametersException(_property.Name);
            }
            return p2.AddParameter(path,
                                   newItemParams.MemberType,
                                   newItemParams.Position);
        }
    }
}