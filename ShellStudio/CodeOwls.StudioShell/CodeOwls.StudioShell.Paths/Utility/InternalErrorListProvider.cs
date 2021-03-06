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
using Microsoft.VisualStudio.Shell;

namespace CodeOwls.StudioShell.Paths.Utility
{
    internal class InternalErrorListProvider : ErrorListProvider
    {
        private const string InternalProviderName = "StudioShell Error Provider";
        private static readonly Guid InternalProviderGuid = new Guid("4ca54b94-ed28-429e-b7b3-bf2f3a356eb3");

        public InternalErrorListProvider(IServiceProvider provider) : base(provider)
        {
            this.ProviderGuid = InternalProviderGuid;
            this.ProviderName = InternalProviderName;
        }
    }
}