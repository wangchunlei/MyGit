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
	[Parameter( ValueFromPipeline = $true )]
	[Object] 
	$InputObject, 

	[Parameter()]
	[scriptblock] 
	# the conditional to apply to each input element; defaults to {$_}
	$predicate={ $_ } 
)  

begin  
{  
	$script:found = $false;  
	$script:item = $null;  
}  
process  
{  
	if( ! $script:found -and ( &$predicate ) )  
	{  
		$script:found = $true;  
        $script:item = $inputObject;  
	}  
}  
end  
{  
        return $script:item;  
}   

<#
.SYNOPSIS
Identifies the first element in the pipeline that meets the specified criteria.

.DESCRIPTION
Processes each input item until the script block specified in the predicate parameter has been satisfied.

Once the predicate returns a true value, the elements are discarded and the script block is no longer invoked.

.INPUTS
System.Object[].  A collection of objects.

.OUTPUTS
System.Object.  The first object that meets the criteria specified in the predicate script block.

.EXAMPLE
C:\PS> $c = 'this is the only value'
C:\PS> @($a,$b,$c,$d) | select-first
'this is the only value'

.EXAMPLE
C:\PS> @(1,2,3,4,5) | select-first { $_ -gt 3 }
4
#>

