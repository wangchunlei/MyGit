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
using System.Text;
using System.Text.RegularExpressions;

namespace CodeOwls.StudioShell.Utility
{
    static class FunctionUtilities
    {
        private static string StudioShellScriptCommandPattern = "^codeowls\\.studioshell\\.connect\\.";
        static internal string GetFunctionNameFromPath(string path)
        {
            if (!IsPowerShellCommandName( path ))
            {
                path = "CodeOwls.StudioShell.Connect." + path;
            }

            return path;
        }

        public static bool IsPowerShellCommandName(string commandName)
        {
            return Regex.IsMatch(commandName, StudioShellScriptCommandPattern, RegexOptions.IgnoreCase);
        }
    }
}
