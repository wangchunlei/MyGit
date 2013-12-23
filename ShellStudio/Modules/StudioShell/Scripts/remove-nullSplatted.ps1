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
	[hashtable] 
	# the table of values to cull
	$a
);

$local:nullValueKeys = $a.Keys | where-object {-not $a[$_]};
$local:nullValueKeys | foreach-object { $a.Remove( $_ ) };

$a;

<#
.SYNOPSIS
Removes any entries with null values from a hashtable.

.DESCRIPTION
Removes any entries with null values from a hashtable.  Useful when 
splatting arguments to a command to remove unnecessary empty or
unspecified argument values.

.INPUTS
System.Collections.Hashtable.  The splatted argument hashtable.

.OUTPUTS
System.Collections.Hashtable.  The splatted argument hashtable with 
unspecified arguments removed.

.NOTES
This cmdlet is used internally by commands that proxy arguments to other commands.

.EXAMPLE
C:\PS> @{ a=1, b=$null} | remove-nullSplatted
Name	Value
----	-----
a		1

.EXAMPLE
C:\PS> $a = @{
		Category = $category;
		Priority = $priority;
		Icon = $icon;
		Checkable = $checkable;
		File = $file;
		Line = $line;
		ReadOnly = $readOnly;
		NoFlush = $noFlush;
	} | remove-nullSplatted;
C:\PS> new-item dte:tasks -value $inputObject @a | out-null;

This example uses splatting to proxy local variables to the new-item cmdlet.  The remove-nullSplatted cmdlet ensures that no unspecified options are passed to the new-item cmdlet.
#>

