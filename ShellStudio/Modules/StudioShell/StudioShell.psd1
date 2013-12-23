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
#	StudioShell module definition file
#

@{

# Script module or binary module file associated with this manifest
ModuleToProcess = 'bin/codeowls.studioshell.provider.dll'

# Version number of this module.
ModuleVersion = '1.5.0.0'

# ID used to uniquely identify this module
GUID = '8714c9a1-1644-4770-9a7d-136f2907d153'

# Author of this module
Author = 'Jim Christopher'

# Company or vendor of this module
CompanyName = 'Code Owls LLC'

# Copyright statement for this module
Copyright = 'Copyright (c) 2011 Code Owls LLC, All Rights Reserved'

# Description of the functionality provided by this module
Description = 'Enables access to Visual Studio Extensibility features in a deeply integrated way.'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '2.0'

# Name of the Windows PowerShell host required by this module
PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
PowerShellHostVersion = ''

# Minimum version of the .NET Framework required by this module
DotNetFrameworkVersion = ''

# Minimum version of the common language runtime (CLR) required by this module
CLRVersion = ''

# Processor architecture (None, X86, Amd64, IA64) required by this module
ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = @()

# Script files (.ps1) that are run in the caller's environment prior to importing this module
ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
FormatsToProcess = 'formats\_Root.Formats.ps1xml', 
				'formats\AddIns.Formats.ps1xml',
				'formats\CodeModel.Formats.ps1xml', 
               'formats\CommandBars.Formats.ps1xml', 
               'formats\Commands.Formats.ps1xml', 
               'formats\Debugger.Formats.ps1xml',
               'formats\Errors.Formats.ps1xml',
               'formats\FontsAndColors.Formats.ps1xml', 
               'formats\OutputPanes.Formats.ps1xml',
               'formats\Project.Formats.ps1xml', 
               'formats\Settings.Formats.ps1xml', 
               'formats\Tasks.Formats.ps1xml',
			   'formats\Templates.Formats.ps1xml',
               'formats\Window.Formats.ps1xml', 
               'formats\WindowConfiguration.Formats.ps1xml'

# Modules to import as nested modules of the module specified in ModuleToProcess
NestedModules = 'bin/codeowls.studioshell.cmdlets.dll',
				'start-studioshell.psm1'

# Functions to export from this module
FunctionsToExport = '*'

# Cmdlets to export from this module
CmdletsToExport = '*'

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module
AliasesToExport = '*'

# List of all modules packaged with this module
ModuleList = @()

# List of all files packaged with this module
FileList = @()

# Private data to pass to the module specified in ModuleToProcess
PrivateData = ''

}

