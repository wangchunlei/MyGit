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
	[parameter(mandatory=$true,ValueFromPipeline=$true)]
	[string] 
	# the file to open
	$file
);

process
{
	$dte.ItemOperations.OpenFile( $file );
}

<#
.SYNOPSIS 
Opens a file in the Visual Studio environment.

.DESCRIPTION
Open the supplied file path in the appropriate default editor.

This command operates on file paths only; PSDTE provider paths are not supported.

.INPUTS
System.String.  The file path to open.

.OUTPUTS
None.

.EXAMPLE
C:\PS> open-file 'myfile.cs'

This example opens the file at c:\ps\myfile.cs in the source code editor.

.EXAMPLE
C:\PS> ls | open-file;

This example opens every file in the c:\ps folder, using the appropriate editor for each individual file type.
#>

