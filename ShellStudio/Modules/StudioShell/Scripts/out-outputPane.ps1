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
	[string] 
	[parameter( Mandatory=$false )]
	# the name of the output pane where output will be sent; defaults to 'StudioShell'
	$name = 'StudioShell',
	
	[string]
	[parameter( mandatory=$true, valueFromPipeline=$true )]
	# the string to write to the output pane
	$inputObject
);

process
{
	$pane = $null;
	$path = "dte:/outputPanes/$name";
	if( -not( test-path $path ) )
	{
		write-debug "creating new output pane at $path";
		$pane = new-item $path;
	}

	$pane = get-item $path;

	if( -not( $inputObject -match '[`r`n]+$' ) )
	{
		$inputObject += "`n";
	}	
	
	$pane.outputString( ( $inputObject -replace ,'' ) );
}

<#
.SYNOPSIS 
Writes a string to an output pane.

.DESCRIPTION
Writes a string to a named output window pane.  

By default, output is written to the 'StudioShell' output window pane.  You can specify a different output window pane using the -name parameter.

.INPUTS
A string value to write to the output pane.

.OUTPUTS
None.

.EXAMPLE
C:\PS> get-date | out-outputpane;


#>
