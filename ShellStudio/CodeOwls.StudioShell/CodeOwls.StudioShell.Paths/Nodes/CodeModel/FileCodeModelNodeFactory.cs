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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using CodeOwls.PowerShell.Provider.Attributes;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Common.Exceptions;
using CodeOwls.StudioShell.Common.Utility;
using CodeOwls.StudioShell.Paths.Items;
using CodeOwls.StudioShell.Paths.Utility;
using EnvDTE;
using EnvDTE80;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace CodeOwls.StudioShell.Paths.Nodes.CodeModel
{
    [CmdletHelpPathID("CodeFile")]
    public class FileCodeModelNodeFactory : NodeFactoryBase, INewItem
    {
        private readonly FileCodeModel _codeModel;

        public FileCodeModelNodeFactory(FileCodeModel codeModel)
        {
            _codeModel = codeModel;
        }

        public override string Name
        {
            get { return NodeNames.CodeModel; }
        }

        #region INewItem Members

        public IEnumerable<string> NewItemTypeNames
        {
            get { return CodeItemTypes.FileCodeModelItemTypes; }
        }

        public object NewItemParameters
        {
            get { return new NewCodeElementItemParams(); }
        }

        #endregion

        public override IPathNode GetNodeValue()
        {
            return new PathNode(new ShellContainer(this), Name, true);
        }

        public override IEnumerable<INodeFactory>  GetNodeChildren( IContext context )
        {
            foreach (CodeElement e in _codeModel.CodeElements)
            {
                var factory = CreateNodeFactoryFromCodeElement(e);
                yield return factory;
            }
        }

        internal static CodeElementNodeFactory CreateNodeFactoryFromCodeElement(CodeElement element)
        {
            if (element is CodeNamespace)
            {
                return new CodeNamespaceNodeFactory(element as CodeNamespace);
            }
            if (element is CodeClass2)
            {
                return new CodeClassNodeFactory(element as CodeClass2);
            }
            if (element is CodeInterface2)
            {
                return new CodeInterfaceNodeFactory(element as CodeInterface2);
            }
            if (element is CodeProperty)
            {
                return new CodePropertyNodeFactory(element as CodeProperty);
            }
            if (element is CodeFunction2)
            {
                return new CodeMethodNodeFactory(element as CodeFunction2);
            }
            if (element is CodeEvent)
            {
                return new CodeEventNodeFactory(element as CodeEvent);
            }
            if (element is CodeVariable2)
            {
                return new CodeVariableNodeFactory(element as CodeVariable2);
            }
            if (element is CodeEnum)
            {
                return new CodeEnumNodeFactory(element as CodeEnum);
            }
            if (element is CodeAttribute2)
            {
                return new CodeAttributeNodeFactory(element as CodeAttribute2);
            }
            if (element is CodeDelegate2)
            {
                return new CodeDelegateNodeFactory(element as CodeDelegate2);
            }
            if (element is CodeParameter2)
            {
                return new CodeParameterNodeFactory(element as CodeParameter2);
            }
            if (element is CodeAttributeArgument)
            {
                return new CodeAttributeArgumentNodeFactory(element);
            }
            if (element is CodeStruct2)
            {
                return new CodeStructNodeFactory(element as CodeStruct2);
            }

            return new CodeElementNodeFactory(element);
        }

        public static CodeElementNodeFactory CreateNodeFactoryFromCurrentSelection(DTE2 dte,
                                                                                     vsCMElement codeElementType)
        {
            if (null == dte.ActiveDocument || null == dte.ActiveDocument.Selection)
            {
                return null;
            }

            TextSelection selection = dte.ActiveDocument.Selection as TextSelection;
            if (null == selection)
            {
                return null;
            }

            TextPoint textPoint = selection.ActivePoint;
            if (null == textPoint)
            {
                return null;
            }

            return CreateNodeFactoryFromTextPoint(textPoint, codeElementType);
        }

        internal static CodeElementNodeFactory CreateNodeFactoryFromTextPoint(TextPoint textPoint,
                                                                              vsCMElement codeElementType)
        {
            var codeElement = textPoint.get_CodeElement(codeElementType);
            if (null == codeElement)
            {
                return null;
            }

            var factory = CreateNodeFactoryFromCodeElement(codeElement);
            return factory;
        }

        public IPathNode NewItem(IContext context, string path, string itemTypeName, object newItemValue)
        {
            if (!NewItemTypeNames.Contains(itemTypeName, StringComparer.InvariantCultureIgnoreCase))
            {
                WriteNotSupportedItemTypeError("file", context, itemTypeName, path);

                return null;
            }
            object item = null;
            var p = context.DynamicParameters as NewCodeElementItemParams;
            switch (itemTypeName.ToLowerInvariant())
            {
                case CodeItemTypes.Attribute:
                    item = NewAttribute(p, path, newItemValue);
                    break;
                case CodeItemTypes.Class:
                    item = NewClass(p, path);
                    break;
                case CodeItemTypes.Delegate:
                    item = NewDelegate(p, path);
                    break;
                case CodeItemTypes.Enum:
                    item = NewEnum(p, path);
                    break;
                case CodeItemTypes.Import:
                    item = NewImport(p, path, newItemValue);
                    break;
                case CodeItemTypes.Method:
                    item = NewMethod(p, path);
                    break;
                case CodeItemTypes.Property:
                    item = NewProperty(p, path);
                    break;
                case CodeItemTypes.Interface:
                    item = NewInterface(p, path);
                    break;
                case CodeItemTypes.Namespace:
                    item = NewNamespace(p, path);
                    break;
                case CodeItemTypes.Struct:
                    item = NewStruct(p, path);
                    break;
                case CodeItemTypes.Variable:
                    item = NewVariable(p, path);
                    break;
            }

            var shellItem = ShellObjectFactory.CreateFromCodeElement(item as CodeElement);
            return new PathNode(shellItem, shellItem.Name, shellItem.IsContainer);
        }

        private object NewImport(NewCodeElementItemParams newItemParams, string path, object newItemValue)
        {
            FileCodeModel2 fileCodeModel2 = _codeModel as FileCodeModel2;
            if (null == fileCodeModel2)
            {
                return null;
            }

            // apparantly required to prevent an exception in VS
            newItemParams.Position = Math.Max(newItemParams.Position, 0);

            return fileCodeModel2.AddImport(path, newItemParams.Position, String.Empty);
        }

        private object NewVariable(NewCodeElementItemParams newItemParams, string path)
        {
            return _codeModel.AddVariable(path,
                                          newItemParams.MemberType,
                                          newItemParams.Position,
                                          newItemParams.Access.ToCMAccess());
        }

        private object NewProperty(NewCodeElementItemParams newItemParams, string path)
        {
            var kind = vsCMFunction.vsCMFunctionPropertyGet;
            if (newItemParams.Set.IsPresent)
            {
                kind |= vsCMFunction.vsCMFunctionPropertySet;
            }

            return _codeModel.AddFunction(path,
                                          kind,
                                          newItemParams.MemberType,
                                          newItemParams.Position,
                                          newItemParams.Access.ToCMAccess()
                );
        }

        private object NewStruct(NewCodeElementItemParams newItemParams, string path)
        {
            return _codeModel.AddStruct(path,
                                        newItemParams.Position,
                                        newItemParams.Bases.ToCSVDTEParameter(),
                                        newItemParams.ImplementedInterfaces.ToCSVDTEParameter(),
                                        newItemParams.Access.ToCMAccess());
        }

        private object NewNamespace(NewCodeElementItemParams newItemParams, string path)
        {
            return _codeModel.AddNamespace(path,
                                           newItemParams.Position);
        }

        private object NewInterface(NewCodeElementItemParams newItemParams, string path)
        {
            return _codeModel.AddInterface(path,
                                           newItemParams.Position,
                                           newItemParams.Bases.ToCSVDTEParameter(),
                                           newItemParams.Access.ToCMAccess());
        }

        private object NewMethod(NewCodeElementItemParams newItemParams, string path)
        {
            return _codeModel.AddFunction(path,
                                          newItemParams.MethodType.ToCMFunction(),
                                          newItemParams.MemberType,
                                          newItemParams.Position,
                                          newItemParams.Access.ToCMAccess());
        }

        private object NewEnum(NewCodeElementItemParams newItemParams, string path)
        {
            return _codeModel.AddEnum(path,
                                      newItemParams.Position,
                                      newItemParams.Bases.ToCSVDTEParameter(),
                                      newItemParams.Access.ToCMAccess()
                );
        }

        private object NewDelegate(NewCodeElementItemParams newItemParams, string path)
        {
            return _codeModel.AddDelegate(path,
                                          newItemParams.MemberType,
                                          newItemParams.Position,
                                          newItemParams.Access.ToCMAccess()
                );
        }

        private object NewClass(NewCodeElementItemParams newItemParams, string path)
        {
            return _codeModel.AddClass(path,
                                       newItemParams.Position,
                                       newItemParams.Bases.ToCSVDTEParameter(),
                                       newItemParams.ImplementedInterfaces.ToCSVDTEParameter(),
                                       newItemParams.Access.ToCMAccess()
                );
        }

        private object NewAttribute(NewCodeElementItemParams p, string path, object newItemValue)
        {
            var value = null == newItemValue ? null : newItemValue.ToString();
            return _codeModel.AddAttribute(path,
                                           value,
                                           p.Position.ToDTEParameter());
        }

        private void WriteNotSupportedItemTypeError(string thisCodeItemTypeName, IContext context, string itemTypeName,
                                                    string path)
        {
            context.WriteError(
                new ErrorRecord(
                    new CodeModelNodeDoesNotSupportItemTypeException(thisCodeItemTypeName, itemTypeName),
                    "StudioShell.NewItem.InvalidItemType",
                    ErrorCategory.InvalidArgument,
                    path
                    ));
        }
    }
}