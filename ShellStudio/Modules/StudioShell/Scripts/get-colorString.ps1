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
	[Parameter( Mandatory = $true, ValueFromPipeline = $true )]
	[System.Drawing.Color] 
	# the color value to convert 
	$color 
)

process
{
	function get-ColorKey( [System.Drawing.Color] $c )
	{
		"RBG({0},{1},{2})" -f $c.R, $c.G, $c.B;
	}


	if( $color.IsNamedColor ) 
	{
		return $color.Name;
	}

	if( ! $global:ss_colorMap )
	{
		write-debug 'initializing color name map...';
		
		$global:ss_colorMap = @{};

		[system.drawing.color] | 
			get-member -static -mem properties | 
			foreach-object { 
				$c = invoke-expression "[system.drawing.color]::$($_.Name)";
				$name = $c.Name;
				if( 0 -eq $name )
				{
					$name = 'Black';
				}
				$global:ss_colorMap[ ( get-ColorKey $c ) ] = $name;		
			};
			
		write-debug 'done initializing color name map';
	}

	$local:colorKey = ( get-colorKey $color );
	write-debug "looking up $($local:colorKey) in color map...";
	$local:colorName = $global:ss_colorMap[ $local:colorKey ];
	write-debug "found $($local:colorName) value in color map...";
	@($local:colorName,$local:colorKey) |select-first;
}

<#
.SYNOPSIS 
Converts a color object to a readable string.

.DESCRIPTION
Converts a color object to its name value.  If no name exists for the color, an RGB() representation is created.

This cmdlet is used by the StudioShell format files to display color values.

.INPUTS
A System.Drawing.Color value.

.OUTPUTS
System.String. The color name, or an RGB() representation.

.EXAMPLE
C:\PS> [System.Drawing.Color]::FromArbg( 255,25,25,112 ) | get-colorString
MidnightBlue

.EXAMPLE
C:\PS> [System.Drawing.Color]::FromArbg( 255,24,25,112 ) | get-colorString
RGB(24,25,112)


.LINK
http://msdn.microsoft.com/en-us/library/14w97wkc.aspx
#>
