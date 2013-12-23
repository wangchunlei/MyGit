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


function push-preference
{
	[CmdletBinding()]
	param( 
		[Parameter( ValueFromPipelineByPropertyName=$true, Mandatory=$true )] $startUpLogLevel 
	)
	
	write-verbose 'pushing current preference state onto the stack';
	$script:preferenceStack.push( @{ debug=$global:debugpreference; verbose=$global:verbosepreference } );
	
	write-verbose "debug: $debugpreference,verbose: $verbosepreference,new log level: $startUpLogLevel";
	
	if( ( 'inquire' -eq $debugpreference ) -or 'none' -ne $startUpLogLevel  )
	{
		write-verbose 'updating global $debugpreference to "continue"';
		$global:debugpreference = 'continue';
	}
	else
	{
		write-verbose 'updating global $debugpreference to "silentlycontinue"';
		$global:debugpreference = 'silentlycontinue';
	}
	if( 'continue' -eq $verbosepreference -or ( 'verbose' -eq $startUpLogLevel   ))
	{
		write-verbose 'updating global $verbosepreference to "continue"';
		$global:verbosepreference = 'continue';
	}
	else
	{
		write-verbose 'updating global $verbosepreference to "silentlycontinue"';
		$global:verbosepreference = 'silentlycontinue';
	}
}

function pop-preference
{
	if( 0 -eq $local:preferenceStack.length )
	{
		return;
	}
	
	$script:top = $script:preferenceStack.pop();
	
	$global:debugpreference = $script:top.debug;
	$global:verbosepreference = $script:top.verbose;	
}

$script:preferenceStack = new-object system.collections.stack;
