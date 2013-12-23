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


[CmdletBinding()]
param(
	[parameter( Mandatory=$false )]
	[switch] 
	# if specified, returns a list of all available path topology versions
	$listAll
);

process
{	
	if( $listAll )
	{
		[CodeOwls.StudioShell.Common.Configuration.PathTopologyVersions]::GetAll();
	}
	else
	{		
		$drive = get-psdrive dte;
		$drive.PathTopologyVersion;
	}
}

<#
.SYNOPSIS 
Returns the current DTE drive path topology version in-use by StudioShell, or
list all available path topology version numbers in your version of 
StudioShell.

.DESCRIPTION
The layout of the DTE drive may change between releases of StudioShell.  This 
cmdlet provides a way to keep scripts compatible across different versions
of StudioShell.

Use this cmdlet to retrieve a list of path topology version numbers available
in your version of StudioShell.  Specify the -listAll parameter to retrieve the
list of path topology version numbers.

If the -listAll parameters is not specified, then this cmdlet will return the
path topology version currently being used by StudioShell.  You can change the
version in-use using the use-pathTopologyVersion cmdlet.

.INPUTS
None.

.OUTPUTS
None.

.EXAMPLE
C:\PS> $p = get-pathTopologyVersion

This example stores the current path topology version number in the variable $p.

.EXAMPLE
C:\PS> get-pathTopologyVersion -listall

This example lists all known path topology versions.

.LINK
use-PathTopologyVersion

.LINK
PSDTE

.LINK
about_StudioShell_Version

.LINK
about_StudioShell_Path

.LINK
about_StudioShell_SolutionModules
#>

