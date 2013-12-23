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

$local:root = $myInvocation.MyCommand.Path | split-path;
write-debug "local module root path is $local:root"

Write-Debug "updating environment...";

$env:Path += ";$($local:root)\bin\Scripts";
$env:PSModulePath += ";$($local:root)";

import-module PreferenceStack;
get-studioShellSettings.ps1 | push-preference;

$cmdline = [environment]::commandLine;
if( $cmdline -match 'powershell(_ise)?\.exe' )
{
	Write-Debug "Commandline [$cmdline] appears to be a general powershell console";

	# bootstrap assemblies and environment  changes
	ls "$($local:root)\bin\*.dll" | foreach { 
		write-debug "loading assembly $_";
		[reflection.assembly]::loadFrom( $_.fullName ) | Out-Null; 
	}
	[CodeOwls.StudioShell.Host.EnvironmentConfiguration]::UpdateEnvironmentForModule();

	$typeName = 'EnvDTE80.DTE2';
	if( -not $dte )
	{
		$dte = [CodeOwls.StudioShell.Common.IoC.Locator]::Get( $typeName )
	}

	if( -not $dte )
	{
		Write-Warning "Bootstrapping a new instance of Visual Studio.  Please be patient as this may take a little while...";
		$dte = new-object -com 'VisualStudio.DTE';
		[CodeOwls.StudioShell.Common.IoC.Locator]::Set( $typeName, $dte );				

		$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
			write-debug 'closing bootstrapped Visual Studio instance.'
			$dte.Quit();
			$dte = $null;
		}
	}
}

if( ( $cmdline -match "devenv.exe" -or [environment]::$cmdline -match "ssms.exe" ) )
{	
	try
	{
		write-debug 'testing for presence of StudioShell add-in...';
		[CodeOwls.StudioShell.Connect] | out-null;
	}
	catch
	{
		throw "The StudioShell Add-In is not loaded.  Please make sure you have the add-in enabled.";
	}
}


# import the assembly into the module session so we can register the runspace with the add-in
write-debug "connecting to StudioShell add in instance";
[CodeOwls.StudioShell.Connect]::RegisterRunspace( $host.runspace );

# create module commands for each script under the scripts directory
# content of each script file becomes a unique exported module function
write-debug "generating module commands";

ls $local:root/scripts | foreach {
	$local:c = ( get-content -path $_.pspath ) -join "`n";	
	$local:n = $_.Name -replace '.{4}$','';
	
	$local:fxn = "function $local:n`n{`n$local:c`n}";
	
	write-verbose $local:fxn
	invoke-expression $local:fxn;
}

# roundabout fix for NuGet weirdness
if( $host.Name -match 'Package Manager Host' )
{
	[CodeOwls.StudioShell.Connect]::PrepareCommandsForCustomHost();
}

pop-preference;
remove-module preferencestack;

