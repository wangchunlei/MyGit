﻿/*
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
using CodeOwls.PowerShell.Host.Executors;

namespace CodeOwls.PowerShell.Host.AutoComplete
{
    internal class CommandListAutoCompleteProvider : CommandAutoCompleteProvider
    {
        private const string Command = @"get-command '{0}'";

        public CommandListAutoCompleteProvider(Executor executor) : base(Command, executor)
        {
        }

        public override IEnumerable<string> GetSuggestions(string guess)
        {
            // apply only when the guess is the first word in the line
            var words = BreakIntoWords(guess);
            if (1 != words.Count())
            {
                return new string[] {};
            }

            return base.GetSuggestions(guess);
        }
    }
}
