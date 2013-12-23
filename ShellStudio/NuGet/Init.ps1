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
param($installPath, $toolsPath, $package, $project)

# verify Visual Studio version support
$vsversion = ( [environment]::getCommandLineArgs() | 
        select -first 1 | 
        get-item |
        select -exp VersionInfo |
        select -exp ProductVersion 
    ) -split '\.' | select -first 1
    
if( 9,10,11 -notcontains $vsversion )
{
    uninstall-package $package.id
    write-error "The current version of Visual Studio ($vsversion) is not supported";
    return;
}

$vsVersionName = @{ '9' = "2008"; '10'='2010'; '11'='2012' }[ $vsversion ]

# verify or create home-based module path
$mydocs = [environment]::getFolderPath( 'mydocuments' );
$modulePath = $env:PSModulePath -split ';' -match [regex]::escape( $mydocs ) | select -First 1;
if( -not $modulePath )
{
	$modulePath = "$mydocs/windowspowershell/modules";
	if( -not( Test-Path $modulePath ) )
	{
		mkdir $modulePath;
	}
	
	$modulePath = Resolve-Path $modulePath;
	$env:PSModulePath = '"{0}";{1}' -f $modulePath,$env:PSModulePath;
}

# configure specs - locations of files to be installed
$addinSpec = join-path $toolspath -child "StudioShell/bin/StudioShell.VS${vsversionname}.AddIn";
$settingsSpec = join-path $toolspath -child "StudioShell/bin/UserProfile/settings.txt";
$profileSpec = join-path $toolspath -child "StudioShell/bin/UserProfile/profile.ps1";
$addinAssemblyPath = join-path $modulePath -child "StudioShell/bin/CodeOwls.StudioShell.dll";

# configure paths - destinations for spec file installations.
$addinFolder = join-path $env:VisualStudioDir -child "Addins";
$addinFilePath = join-path $addinFolder -child "StudioShell.addin";
$studioShellProfileFolder = $mydocs | join-path -child 'CodeOwlsLLC.StudioShell';
$profilePath = $mydocs | join-path -child 'CodeOwlsLLC.StudioShell/profile.ps1';
$settingsPath = $mydocs | join-path -child 'CodeOwlsLLC.StudioShell/settings.txt';

$packageVersion = $package.version.version;
    
write-debug "Home Path: $home";
Write-Debug "Tools Path: $toolspath"
Write-Debug "Module Path: $modulePath"
Write-Debug "AddIn Folder: $addinFolder"
Write-Debug "AddIn File Path: $addinFilePath"
Write-Debug "StudioShell Profile Folder: $studioShellProfileFolder"
Write-Debug "StudioShell Profile Path: $profilePath"
Write-Debug "StudioShell Settings Path: $settingsPath"


write-debug "Attempting install of StudioShell package version $packageVersion";

function test-studioShellImported
{    
    $existingModuleVersion = $null;
    $loadedAssembly = [appdomain]::currentDomain.getAssemblies() | where { $_.fullname -match 'codeowls\.studioshell' } | select -first 1;
    $existingModule = get-module studioshell;
    
    if( $loadedAssembly )
    {
        $existingModuleVersion = ( new-object system.reflection.assemblyname -arg $loadedAssembly.FullName ).Version;
        write-debug "Found loaded assembly $loadedAssembly; existing module version is $existingModuleVersion"
    }
    elseif( $existingModule )
    {
        $existingModuleVersion = $existingModule.Version;
        write-debug "Found loaded module $existingModule; existing module version is $existingModuleVersion"
    }
    
    if( $existingModuleVersion )
    {        
        write-debug "A version of StudioShell is already imported: ${existingModuleVersion}";
        if( $existingModuleVersion -ge $packageVersion )
        {
            write-debug "Imported version of StudioShell module/assembly is equal to or newer than package version";
            import-studioshell;
            return $true;
        }
        
    	Remove-Item $addinFilePath -force -erroraction 'silentlycontinue';
        uninstall-package $package.id;

        write-error @"
The StudioShell package version ${packageVersion} cannot be installed because you already have version ${existingModuleVersion} of the StudioShell module and assemblies loaded in this Visual Studio process.
The StudioShell ${existingModuleVersion} addin has been disconnected from Visual Studio.  
Please restart all currently open instances of Visual Studio and install StudioShell ${packageVersion} again.
"@;
        return $true;        
    }
    
    return $false;
}

function test-higherVersionStudioShellInstalled
{
    $existingModule = get-module -list StudioShell | select -first 1;
    if( $existingModule )
    {
        $existingModuleVersion = $existingModule.version;
        write-debug "A version of StudioShell is already installed: ${existingModuleVersion}";
        if( $existingModuleVersion -ge $packageVersion )
        {            
            if( $existingModuleVersion -gt $packageVersion )
            {
                write-debug "Existing StudioShell version ${existingModuleVersion} is higher than package version ${packageVersion}; uninstalling package";
                uninstall-package $package.id;
                            
                write-warning @"
You are attepting to install StudioShell version ${packageVersion}, but a higher version ${existingModuleVersion} is already installed.
The higher version of StudioShell will be imported into this session.  
"@;
            }
            
            $modulebase = $existingModule.ModuleBase;
            $addinSpec = join-path $modulebase -child "bin/StudioShell.VS${vsversionname}.AddIn";
            $addinAssemblyPath = join-path $modulebase -child "bin/CodeOwls.StudioShell.dll";
            
            write-debug "AddIn spec file path is now: $addinspec"
            write-debug "AddIn assembly file path is now: $addinassemblypath"
            
            write-addInFile;
            connect-StudioShellAddIn;
            
            import-studioshell;
            return $true;        
        }
    }
    
    return $false;
}

function define-connectorType 
{
    try
	{
		add-type -reference EnvDTE -typedef @'
/*
	Necessary to prevent Visual Studio from crashing when accessing 
	some of the AddIn object properties from PowerShell.  
	In particular, the Instance property seems to set off Visual Studio.
*/
public class AddInConnector
{
    public static void Connect(object dte)
	{
	   EnvDTE.DTE _dte = dte as EnvDTE.DTE;
	   _dte.AddIns.Update();
       foreach( EnvDTE.AddIn addIn in _dte.AddIns ) 
       {
            if( addIn.Name.Contains( "StudioShell" ) && ! addIn.Connected )
			{
			     addIn.Connected = true;
			}
	   }
    }
						
	public static void Disconnect(object dte)
	{
	   EnvDTE.DTE _dte = dte as EnvDTE.DTE;

		foreach( EnvDTE.AddIn addIn in _dte.AddIns )
		{
			if( addIn.Name.Contains( "StudioShell" ) )
			{
			    addIn.Connected = false;
								
    			_dte.AddIns.Update();
                return;
            }
	   }				
    }
}    
'@;
	}
	catch
	{
	}	
}

function connect-StudioShellAddIn
{
    define-connectorType;

    # wire-up the new addin
	Write-Debug 'Connecting StudioShell Add-In'
	[AddInConnector]::Connect( $dte );	
}

function write-addInFile
{
    # create module, profile, and addin directories
    write-debug "Creating folders $modulePath;$studioShellProfileFolder;$addInFolder";
	mkdir $modulePath,$studioShellProfileFolder,$addinFolder -erroraction silentlycontinue;
	
    # create the addin file
    write-debug "Creating addin spec file $addinfilepath"
	( gc $addinSpec ) -replace '<Assembly>.+?</Assembly>',"<Assembly>$addinAssemblyPath</Assembly>" | out-file $addinFilePath;
}

function install-StudioShell
{
    try
    {
    	pushd $env:HOMEDRIVE;
    	
    	Write-Debug "Installing StudioShell module version $($package.Version)..."
           		
        # copy the StudioShell module on to the module path
        write-debug "placing StudioShell module on module path $modulepath"
    	cp "$toolsPath/StudioShell" -Destination $modulePath -container -Recurse -force
    		
        # create new settings file if one doesn't already exist
    	if( -not( Test-Path $settingsPath ) )
    	{
            write-debug "creating $settingsPath"
    		cp $settingsSpec $settingsPath;
    	}
        
        # create new profile script if one doesn't already exist
    	if( -not( Test-Path $profilePath ) )
    	{
            write-debug "creating $profilePath"
    		cp $profileSpec $profilePath
    	}

        write-addInFile;
        connect-StudioShellAddIn;
    }
    finally
    {
    	popd;
    }
}

function import-StudioShell
{
    if( get-module studioshell )
    {
        return;
    }
    
    # pull in the studioshell module
    Write-Debug 'Importing StudioShell module'
    import-module "StudioShell";
}

if( ( test-studioShellImported ) -or ( test-higherVersionStudioShellInstalled ))
{
    write-debug 'bypassing install - StudioShell is already imported or installed';
    return;    
}

install-StudioShell
import-StudioShell


