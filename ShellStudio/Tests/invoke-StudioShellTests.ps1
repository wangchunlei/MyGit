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
param( $fixture = '*.Tests.ps1', [switch] $keepSolution )

if( -not( get-module -list pester ) )
{
    throw "StudioShell tests require the Pester module"
}

# dotsource helper functions for tests
. ./helpers.ps1

# use pester
ipmo pester;

# run the tests
invoke-pester $fixture

# remove the dangling solution if necessary
if( ! $keepSolution ) { delete-solution; }

# focus the studioshell console if we're in VS
if( [environment]::commandline -match 'devenv\.exe' ) 
{ 
    invoke-item dte:/windows/studioshell 
};

