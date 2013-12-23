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

function do-updateHeaders( $headerPrefix, $pattern )
{
	$header = ( ( gc ( gi dte:/solution/projects/solutionresources/${headerPrefix}header.txt | select -exp filename ) ) -join "`r`n" ).trim();
	$root = ( gi dte:/solution ).fullname | split-path;

	write-host "finding all $headerPrefix code files ..."
	$files = ls $root -rec -inc $pattern | where { $_ -notmatch '_ReSharper' };	
	if( $headerPrefix -eq 'xml' ) { $files | write-host };
	write-host "updating missing code file headers ..."
	$files | where {
		( $_ | gc ) -join '' -notmatch 'ms-rl'
	} | foreach {
		write-host "updating file header in $_"
		$c = (gc $_) -join "`r`n";  
		$c = $header + "`r`n" + $c; 
		$c | out-file $_ -encoding 'UTF8';
		$c = $null;
	}

	$header = $null;
}

do-updateHeaders 'cs' '*.cs';
do-updateHeaders 'ps' '*.ps1','*.psm1','*.psd1';
do-updateHeaders 'xml' '*.xml','*.resx','*.addin','*.nuspec'







