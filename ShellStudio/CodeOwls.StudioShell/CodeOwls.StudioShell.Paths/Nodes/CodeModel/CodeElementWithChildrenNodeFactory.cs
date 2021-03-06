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
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Common.Exceptions;
using CodeOwls.StudioShell.Paths.Items;
using EnvDTE;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace CodeOwls.StudioShell.Paths.Nodes.CodeModel
{
    public abstract class CodeElementWithChildrenNodeFactory : CodeElementNodeFactory, INewItem
    {
        protected CodeElementWithChildrenNodeFactory(CodeElement element) : base(element)
        {
        }

        protected override bool IsCollection
        {
            get { return true; }
        }

        protected abstract string CodeItemTypeName { get; }

        #region INewItem Members

        public abstract IEnumerable<string> NewItemTypeNames { get; }

        public object NewItemParameters
        {
            get { return new NewCodeElementItemParams(); }
        }

        #endregion

        public IPathNode NewItem(IContext context, string path, string itemTypeName, object newItemValue)
        {
            return NewChildItem(CodeItemTypeName, NewItemTypeNames, context, path, itemTypeName, newItemValue);
        }

        protected IPathNode NewChildItem(string thisCodeItemTypeName, IEnumerable<string> newItemTypeNames,
                                         IContext context, string path, string itemTypeName, object newItemValue)
        {
            if (!newItemTypeNames.Contains(itemTypeName, StringComparer.InvariantCultureIgnoreCase))
            {
                WriteNotSupportedItemTypeError(thisCodeItemTypeName, context, itemTypeName, path);

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
                case CodeItemTypes.Event:
                    item = NewEvent(p, path);
                    break;
                case CodeItemTypes.Method:
                    p.MemberType = p.MemberType ?? "void";
                    item = NewMethod(p, path);
                    break;
                case CodeItemTypes.Property:
                    if (!(p.Set || p.Get))
                    {
                        context.WriteVerbose( "using new-item to create a new property element, but neither -get nor -set are supplied; assuming you want both -get and -set");
                        p.Get = p.Set = true;
                    }
                    item = NewProperty(p, path);
                    break;
                case CodeItemTypes.Struct:
                    item = NewStruct(p, path);
                    break;
                case CodeItemTypes.Variable:
                    item = NewVariable(p, path, newItemValue);
                    break;
                case CodeItemTypes.Parameter:
                    item = NewParameter(p, path, newItemValue);
                    break;
                case CodeItemTypes.Namespace:
                    item = NewNamespace(p, path);
                    break;
                case CodeItemTypes.Interface:
                    item = NewInterface(p, path);
                    break;

                default:
                    item = null;
                    break;
            }

            if (null == item)
            {
                WriteNotSupportedItemTypeError(thisCodeItemTypeName, context, itemTypeName, path);

                return null;
            }

            var shellItem = ShellObjectFactory.CreateFromCodeElement(item as CodeElement);
            return new PathNode(shellItem, shellItem.Name, shellItem.IsContainer);
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

        protected virtual object NewNamespace(NewCodeElementItemParams newItemParams, string path)
        {
            return null;
        }

        protected virtual object NewInterface(NewCodeElementItemParams newItemParams, string path)
        {
            return null;
        }

        protected virtual object NewEvent(NewCodeElementItemParams newCodeElementItemParams, string path)
        {
            return null;
        }

        protected virtual object NewParameter(NewCodeElementItemParams newItemParams, string path, object newItemValue)
        {
            return null;
        }

        protected virtual object NewVariable(NewCodeElementItemParams newItemParams, string path, object newItemValue)
        {
            return null;
        }

        protected virtual object NewProperty(NewCodeElementItemParams newItemParams, string path)
        {
            return null;
        }

        protected virtual object NewStruct(NewCodeElementItemParams newItemParams, string path)
        {
            return null;
        }

        protected virtual object NewMethod(NewCodeElementItemParams newItemParams, string path)
        {
            return null;
        }

        protected virtual object NewEnum(NewCodeElementItemParams newItemParams, string path)
        {
            return null;
        }

        protected virtual object NewDelegate(NewCodeElementItemParams newItemParams, string path)
        {
            return null;
        }

        protected virtual object NewClass(NewCodeElementItemParams newItemParams, string path)
        {
            return null;
        }

        protected virtual object NewAttribute(NewCodeElementItemParams p, string path, object newItemValue)
        {
            return null;
        }

        protected virtual object NewImport(NewCodeElementItemParams p, string path, object newItemValue)
        {
            return null;
        }
    }
}