#
#   Copyright (c) 2011 Code Owls LLC, All Rights Reserved.
#
#   Licensed under the Microsoft Reciprocal License (Ms-RL) (the "License");
#   you may not use this file except in compliance with the License.
#   You may obtain a copy of the License at
#
#     http://www.opensource.org/licenses/ms-rl
#
#   Unless required by applicable law or agreed to in writing, software
#   distributed under the License is distributed on an "AS IS" BASIS,
#   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#   See the License for the specific language governing permissions and
#   limitations under the License.
#


[CmdletBinding(DefaultParameterSetName='byversion')]
param( 
	[parameter( Mandatory=$true, ParameterSetName="byversion", Position=1 )]
	[version] 
	# the version of StudioShell path topology to use
	$version,

	[parameter( Mandatory=$true, ParameterSetName="bycurrent" )]
	[switch] 
	# if specified, uses the path topology for the installed version of StudioShell
	$current
);

process
{
	$drive = get-psdrive dte;

	if( $current )
	{
		$version = $drive.DefaultPathTopologyVersion;
	}

	$drive.PathTopologyVersion = $version;
}

<#
.SYNOPSIS 
Changes the DTE drive path topology version used by StudioShell.

.DESCRIPTION
The layout of the DTE drive may change between releases of StudioShell.  This 
cmdlet provides a way to keep scripts compatible across different versions
of StudioShell.

For instance, the 1.0 release of StudioShell supports a code model tree under 
each project item:

dte:/solution/projects/MyProject/Program.cs/CodeModel

To facilitate code searches and isolate project properties from the code model,
the code model was moved to its own hive under the solution node in the 1.2 
release of StudioShell:

dte:/solution/CodeModel/MyProject/Program.cs

If you have scripts that rely on the 1.0 path model, you can revert the path
topology to the 1.0 version using the use-pathTopologyVersion cmdlet:

use-pathTopologyVersion -version 1.0

The path topology change remains in effect for the duration of the session,
or until explicitly changed by invoking the cmdlet a second time.  

To use the current path topology for the installed version of StudioShell,
invoke this cmdlet with the -current switch:

use-pathTopologyVersion -current

.INPUTS
None.

.OUTPUTS
None.

.NOTES
It is recommended that use of this cmdlet be restricted to solution modules.  If 
you have a solution that relies on an earlier version of StudioShell paths, you 
can easily leverage your solution module and custom scripts my modifying your 
solution module like so:

# this solution uses the 1.0 path model:
use-pathTopologyVersion 1.0;

$m = $MyInvocation.MyCommand.ScriptBlock.Module;
$m.OnRemove = {
	use-pathTopologyVersion -current;
}

.EXAMPLE
C:\PS> use-pathTopologyVersion 1.0

This example changes the DTE path topology to match the 1.0 StudioShell release.

.EXAMPLE
C:\PS> use-pathTopologyVersion -current

This example sets the DTE path topology to match the currently installed StudioShell version.

.LINK
get-pathTopologyVersion

.LINK
PSDTE

.LINK
about_StudioShell_Version

.LINK
about_StudioShell_Path

.LINK
about_StudioShell_SolutionModules
#>

