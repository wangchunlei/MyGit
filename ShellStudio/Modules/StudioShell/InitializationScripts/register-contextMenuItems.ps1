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


pushd "commandBars:/context menus/Project and Solution Context Menus/Project"
if( test-path "mount this project" ) 
{
	remove-item "mount this project";
}
if( test-path 'mount this folder' )
{
	remove-item "Mount This Folder"
}

new-item -name "Mount This Project" -type button -value { 
	$selectedProjects | select -first 1 | %{ 
		pushd "projects:/$($_.Name)"; 
		write-prompt; 
		invoke-item 'windows:/studio shell';
	} 
} | out-null;
new-item -name "Mount This Folder" -type button -value { 
	$selectedProjects | select -first 1 | %{ 
		split-path $_.FullName | pushd;
		write-prompt; 
		invoke-item 'windows:/studio shell';
	} 
} | out-null;
popd

pushd "commandBars:\context menus\Editor Context Menus\code window";
if( test-path "mount this class" ) 
{
	remove-item "mount this class";
}

new-item -name "Mount This Class" -type button -value { 
	pushd dte:/selectedItems/Class;
	write-prompt;
	invoke-item 'windows:/studio shell';	 
} | out-null;
popd

pushd "commandBars:\context menus\Project and Solution Context Menus\item";
if(test-path "mount this code model")
{
	remove-item "mount this code model";
}

new-item -name "Mount this Code Model" -type button -value {
	$selectedProjectItems | select -first 1 | %{
		pushd "projects:/$($_.ContainingProject.Name)/$($_.Name)/CodeModel";
		write-prompt;
		invoke-item 'windows:/studio shell';	 
	};
} | out-null;
popd;


