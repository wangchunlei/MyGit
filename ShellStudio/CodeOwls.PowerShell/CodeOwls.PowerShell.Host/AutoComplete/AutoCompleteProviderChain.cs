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
using System.Linq;

namespace CodeOwls.PowerShell.Host.AutoComplete
{
    internal class AutoCompleteProviderChain : IAutoCompleteProvider
    {
        private readonly IEnumerable<IAutoCompleteProvider> _providers;

        public AutoCompleteProviderChain(params IAutoCompleteProvider[] providers)
        {
            _providers = providers;
        }

        public AutoCompleteProviderChain(IEnumerable<IAutoCompleteProvider> providers)
        {
            _providers = providers;
        }

        #region IAutoCompleteProvider Members

        public IEnumerable<string> GetSuggestions(string guess)
        {
            if (!_providers.Any())
            {
                return new string[] {};
            }

            Queue<IAutoCompleteProvider> queue = new Queue<IAutoCompleteProvider>(_providers);

            List<string> suggestions = new List<string>();
            while (queue.Any() && ! suggestions.Any())
            {
                IAutoCompleteProvider current = queue.Dequeue();
                var currentStuggestions = current.GetSuggestions(guess);
                if (!currentStuggestions.Any())
                {
                    continue;
                }
                suggestions.AddRange(currentStuggestions);
            }

            return suggestions;
        }

        #endregion
    }
}
