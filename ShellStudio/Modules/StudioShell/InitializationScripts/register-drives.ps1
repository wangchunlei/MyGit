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


# mount common drives

$local:driveData = DATA { @'
	commands = dte:/commands
	commandBars = dte:/commandBars
	windows = dte:/windows
	tasks = dte:/tasks
	projects = dte:/solution/projects
'@ | convertfrom-stringdata
};

# define functions for root drive access points
write-debug "mounting common DTE drives ..."

$local:driveData.keys | foreach {
	
	$local:path = $local:driveData[$_];
	$local:driveName = ( $_ + ':' );
	
	write-verbose "mounting drive ${_}: at path $local:path";
	new-psdrive -name $_ -psprovider PSDTE -root $path | out-null;
	
	write-verbose "defining shortcut function ${_}:";
	new-item -path function: -name $local:driveName -value "set-location $($local:driveName)" | out-null;
};

new-item -path function: -name "dte:" -value "set-location dte:" | out-null;
