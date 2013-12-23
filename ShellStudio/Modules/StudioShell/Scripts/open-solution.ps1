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

param(
	[Parameter(Mandatory=$true, Position=0)]
	[string]
	# the file path of the solution to open
	$path
)

if( -not( Test-Path $path ) )
{
	Write-Error -Category ResourceUnavailable -Message "The path [$path] cannot be found.";
	return;
}

Write-Debug "opening solution at path $path";
$dte.Solution.Open( $path ) | Out-Null;

<#
.SYNOPSIS
Opens a solution file (.SLN) in the IDE.

.DESCRIPTION
This function is a wrapper for opening a solution file (.SLN) in the Visual Studio IDE.

.INPUTS
None.

.OUTPUTS
None.

.EXAMPLE
open-solution -path 'c:\projects\myproject.sln'

This example loads the first solution file at the path c:\projects\myproject.sln.

.EXAMPLE
$sln = ls *.sln
open-solution -path $sln

This example loads the first solution file it finds in the current directory.
#>
