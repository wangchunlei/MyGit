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

# notes:
#	this script is useful for moving the .NET framework version of every project
#	in the solution to 3.5 or 4.0.  StudioShell ships with 3.5 compatibility,
#	but VS2010 is not able to debug a 3.5 project with 4.0 loaded in memory.

param( 
	[ValidateSet( "3.5","4.0" )]
	[string] 
	$version = "3.5"
)


ls dte:/solution/projects | where { 
	$_.kind -match 'FAE' #locate all C# projects by their Kind property; see get-projectType.ps1
} | foreach { 
	write-verbose "updating $($_.name) to .NET version $version ...";
	set-item "$($_.pspath)\projectproperties\targetframeworkmoniker" -value ".NETFramework,Version=$version"
}                  


