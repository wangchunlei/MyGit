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


[Enum]::GetNames( [EnvDTE.vsTaskIcon] );

<#
.SYNOPSIS 
Retrieves a list of task icon names.

.DESCRIPTION
This command returns the names from the [EnvDTE.vsTaskIcon] enumeration.

These icons can be specified to certain item cmdlets in the Tasks node of the DTE drive.

.INPUTS
This command accepts no input.

.OUTPUTS
System.String[]. The complete list of icon names.

.LINK
http://msdn.microsoft.com/en-us/library/envdte.vstaskicon%28v=VS.100%29.aspx

.LINK
get-help -path dte:/tasks new-item

.LINK
get-help -path dte:/tasks set-item
#>
