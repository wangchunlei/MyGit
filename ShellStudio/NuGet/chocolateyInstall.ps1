#
#   Copyright (c) 2013 Code Owls LLC, All Rights Reserved.
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

try
{
    $mydocs = [environment]::getFolderPath( 'mydocuments' );
    $packagePath = Split-Path -parent $MyInvocation.MyCommand.Definition;
	$modulePath = $Env:PSModulePath -split ';' -match [regex]::escape($mydocs) | select -First 1;
	
    write-debug "installing StudioShell into local module repository at $modulePath" 
	
    if( test-path "$modulepath/StudioShell" )
    {
        remove-item $modulePath/StudioShell -force -recurse -erroraction 0;
    }
    
	ls $packagePath | Copy-Item -recurse -Destination $modulePath -Force;	
    
	$addInInstallPath = $modulePath | Join-Path -ChildPath "StudioShell\bin";
	
	$settingsSpec = join-path $addInInstallPath -child "UserProfile/settings.txt";
	$profileSpec = join-path $addInInstallPath -child "UserProfile/profile.ps1";
	$addinAssemblyPath = join-path $addInInstallPath -child "CodeOwls.StudioShell.dll";

	pushd $env:HOMEDRIVE;
    try
	{
		$studioShellProfileFolder = "$mydocs/CodeOwlsLLC.StudioShell";
		$profilePath = "$mydocs/CodeOwlsLLC.StudioShell/profile.ps1";
		$settingsPath = "$mydocs/CodeOwlsLLC.StudioShell/settings.txt";

		mkdir $studioShellProfileFolder -erroraction silentlycontinue;

        '2008','2010','2012', '9','10','11' | where { test-path "~/documents/Visual Studio $_" }  | % { 
            $n = @{ '9'='2008'; '10'='2010'; '11'='2012' }[ $_ ], $_ | select -first 1;
            $addinFolder = "$mydocs/Visual Studio $_/Addins";
		    $addinFilePath = join-path $addinFolder -child "StudioShell.addin";
		    $addinSpec = join-path $addInInstallPath -child "StudioShell.VS${n}.AddIn";
        
            mkdir $addinFolder -erroraction silentlycontinue;
		    ( gc $addinSpec ) -replace '<Assembly>.+?</Assembly>',"<Assembly>$addinAssemblyPath</Assembly>" | out-file $addinFilePath;
        }
		if( -not( test-path $settingsPath ) )
        {
            cp $settingsSpec $settingsPath;
        }
        if( -not( test-path $profilePath ) )
        {
		  cp $profileSpec $profilePath
        }
	}
	finally
	{
		popd;
	}
	
    '10','11' | foreach {
	    if( test-path "HKCU:\software\Microsoft\VisualStudio\$_.0\PreloadAddinStateManaged" )
	    {
		    # reset addin registry flags to force a reload of UI extensions
		    Remove-ItemProperty -Path "HKCU:\software\Microsoft\VisualStudio\$_.0\PreloadAddinStateManaged" -Name *StudioShell*;
	    }
    }

  Write-ChocolateySuccess 'studioshell'
} 
catch 
{
  Write-ChocolateyFailure 'studioshell' $($_.Exception.Message)
  throw
}
