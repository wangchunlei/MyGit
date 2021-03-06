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
using CodeOwls.StudioShell.Paths.Nodes.CodeModel;
using EnvDTE;

namespace CodeOwls.StudioShell.Paths.Utility
{
    public static class DTEExtensions
    {
        public static object ToDTEParameter<T>(this T _this)
        {
            if (_this.Equals(default(T)))
            {
                return null;
            }

            return _this;
        }

        public static object ToStringDTEParameter<T>(this T _this)
        {
            if (_this.Equals(default(T)))
            {
                return null;
            }

            return _this.ToString();
        }

        public static object ToCSVDTEParameter(this string[] _this)
        {
            if (null == _this)
            {
                return null;
            }

            return String.Join(",", _this);
        }

        public static vsCMAccess ToCMAccess(this AccessLevel level)
        {
            switch (level)
            {
                case (AccessLevel.Default):
                    return vsCMAccess.vsCMAccessDefault;
                case (AccessLevel.Public):
                    return vsCMAccess.vsCMAccessPublic;
                case (AccessLevel.Internal):
                    return vsCMAccess.vsCMAccessProject;
                case (AccessLevel.Protected):
                    return vsCMAccess.vsCMAccessProtected;
                case (AccessLevel.ProtectedInternal):
                    return vsCMAccess.vsCMAccessAssemblyOrFamily;

                case (AccessLevel.Private):
                default:
                    return vsCMAccess.vsCMAccessPrivate;
            }
        }

        public static vsCMFunction ToCMFunction(this MethodType methodType)
        {
            switch (methodType)
            {
                case (MethodType.Abstract):
                    return vsCMFunction.vsCMFunctionPure;
                case (MethodType.Virtual):
                    return vsCMFunction.vsCMFunctionVirtual;
                case (MethodType.Constructor):
                    return vsCMFunction.vsCMFunctionConstructor;
                case (MethodType.Destructor):
                    return vsCMFunction.vsCMFunctionDestructor;
                case (MethodType.Operator):
                    return vsCMFunction.vsCMFunctionOperator;

                case (MethodType.Method):
                default:
                    return vsCMFunction.vsCMFunctionFunction;
            }
        }
    }
}