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
	[Parameter( HelpMessage = "The task category")]
    [string] 
    # the task category
    $category,
    
    [Parameter(HelpMessage = "The task priority")]
    [EnvDTE.vsTaskPriority] 
    # the task priority
    $priority,
        
    [Parameter(HelpMessage = "The icon to show for the task")]
    [EnvDTE.vsTaskIcon]
    # the icon to show for the task
    $icon,
    
    [Parameter(HelpMessage = "Specifies whether the task displays a checkbox")]
    [Switch] 
    # when specified, the task will contain a checkbox
    $checkable,
    
    [Parameter(HelpMessage = "The file associated with this task")]
    [string] 
    # the file associated with this task
    $file,
    
    [Parameter(HelpMessage = "The file line index associated with this task")]
    [int] 
    # the file line index associated with this task
    $line,
    
    [Parameter(HelpMessage = "Specifies that the task cannot be deleted by the user")]
    [Switch] 
    # when specified, the task cannot be deleted by the user
    $readOnly,
    
    [Parameter(HelpMessage="Indicates that the task should not be immediately flushed to the task pane")]
    [Switch] 
    # when specified, indicates that the task should not be immediately flushed to the task pane
    $noFlush,
	
	[string]
	[parameter( mandatory=$true, valueFromPipeline=$true )]
	# the value to write to the task pane
	$inputObject
);

process
{
	$a = @{
		Category = $category;
		Priority = $priority;
		Icon = $icon;
		Checkable = $checkable;
		File = $file;
		Line = $line;
		ReadOnly = $readOnly;
		NoFlush = $noFlush;
	} | remove-nullSplatted;
	
	new-item dte:tasks -value $inputObject @a | out-null;
}

<#
.SYNOPSIS
Outputs data to the Visual Studio task list.

.DESCRIPTION
Outputs data to the Visual Studio task list.

.INPUTS
String.  The task description to add to the task list.

.OUTPUTS
None.

.EXAMPLE
ls dte:/solution/codemodel -recurse | 
  where {$_ -match 'class'} | 
  select -expand Name | 
  out-tasklist -category 'documentation' -checkable

This example creates a new task for each class found in the currently loaded 
solution.  The task category is 'documentation', the task list provides a 
checkbox for each task, and each task description is set to the name of the 
class.
#>

