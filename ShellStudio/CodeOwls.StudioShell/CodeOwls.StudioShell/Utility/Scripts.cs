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
using CodeOwls.PowerShell.Host.Utility;

namespace CodeOwls.StudioShell.Utility
{
    static class Scripts
    {
        public const string StartStudioShell = @"
if( -not( @('RemoteSigned','Unrestricted') -contains ( get-executionpolicy ) ) )
{
    $local:warn = 
@""
StudioShell cannot start because the execution of script files is disabled on this system.

Local script execution policy must be 'RemoteSigned' or 'Unrestricted' to enable StudioShell.
Please change the script execution policy on this system and restart Visual Studio.

For information about script execution policy, including information on how to change it,
type the following:

get-help about_execution_policies

""@

    write-warning $local:warn;

    return;
}

. start-studioShell.ps1";

        public const string RunProfiles = ". invoke-studioShellProfile.ps1";

        public const string EnableDebugWrite = "$debugpreference = 'continue'";
        public const string DisableDebugWrite = "$debugpreference = 'silentlycontinue'";
        public const string EnableVerboseWrite = "$verbosepreference = 'continue'";
        public const string DisableVerboseWrite = "$verbosepreference = 'silentlycontinue'";

        public const string WarnDefaultConsole = @"write-warning @'
This is the Windows I/O console for your Visual Studio process.

You should assume that doing either of the following:
 * typing ""exit"" in this console;
 * closing this console window
will aggressively end the Visual Studio process, bypassing any option to save your work.

Caveat emptor, the cake is a lie, use with caution, you have been warned....
   
'@";

        public static string CreateRunProfileScript( IProfileInfo profileInfo )
        {
            return String.Format(@"{0} -profile {1}", RunProfiles, profileInfo.ToPSHashtable());
        }
    }
}
