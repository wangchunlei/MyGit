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
	[Parameter( Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true )]
	[string] 
	# a string containing the project kind GUID value
	$kind 
)
process
{
	switch( $kind )
	{
		"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" { "Windows (C#)" }
		"{F184B08F-C81C-45F6-A57F-5ABD9991F28F}" { "Windows (VB.NET)" }
		"{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}" { "Windows (Visual C++)" }
		"{349C5851-65DF-11DA-9384-00065B846F21}" { "Web Application" }
		"{E24C65DC-7377-472B-9ABA-BC803B73C61A}" { "Web Site" }
		"{F135691A-BF7E-435D-8960-F99683D2D49C}" { "Distributed System" }
		"{3D9AD99F-2412-4246-B90B-4EAA41C64699}" { "Windows Communication Foundation (WCF)" }
		"{60DC8134-EBA5-43B8-BCC9-BB4BC16C2548}" { "Windows Presentation Foundation (WPF)" }
		"{C252FEB5-A946-4202-B1D4-9916A0590387}" { "Visual Database Tools" }
		"{A9ACE9BB-CECE-4E62-9AA4-C7E7C5BD2124}" { "Database" }
		"{4F174C21-8C12-11D0-8340-0000F80270F8}" { "Database (other project types)" }
		"{3AC096D0-A1C2-E12C-1390-A8335801FDAB}" { "Test" }
		"{20D4826A-C6FA-45DB-90F4-C717570B9F32}" { "Legacy (2003) Smart Device (C#)" }
		"{CB4CE8C6-1BDB-4DC7-A4D3-65A1999772F8}" { "Legacy (2003) Smart Device (VB.NET)" }
		"{4D628B5B-2FBC-4AA6-8C16-197242AEB884}" { "Smart Device (C#)" }
		"{68B1623D-7FB9-47D8-8664-7ECEA3297D4F}" { "Smart Device (VB.NET)" }
		"{14822709-B5A1-4724-98CA-57A101D1B079}" { "Workflow (C#)" }
		"{D59BE175-2ED0-4C54-BE3D-CDAA9F3214C8}" { "Workflow (VB.NET)" }
		"{06A35CCD-C46D-44D5-987B-CF40FF872267}" { "Deployment Merge Module" }
		"{3EA9E505-35AC-4774-B492-AD1749C4943A}" { "Deployment Cab" }
		"{978C614F-708E-4E1A-B201-565925725DBA}" { "Deployment Setup" }
		"{AB322303-2255-48EF-A496-5904EB18DA55}" { "Deployment Smart Device Cab" }
		"{A860303F-1F3F-4691-B57E-529FC101A107}" { "Visual Studio Tools for Applications (VSTA)" }
		"{BAA0C2D2-18E2-41B9-852F-F413020CAA33}" { "Visual Studio Tools for Office (VSTO)" }
		"{F8810EC1-6754-47FC-A15F-DFABD2E3FA90}" { "SharePoint Workflow" }
		"{6D335F3A-9D43-41b4-9D22-F6F17C4BE596}" { "XNA (Windows)" }
		"{2DF5C3F4-5A5F-47a9-8E94-23B4456F55E2}" { "XNA (XBox)" }
		"{D399B71A-8929-442a-A9AC-8BEC78BB2433}" { "XNA (Zune)" }
		"{EC05E597-79D4-47f3-ADA0-324C4F7C7484}" { "SharePoint (VB.NET)" }
		"{593B0543-81F6-4436-BA1E-4747859CAAE2}" { "SharePoint (C#)" }
		"{A1591282-1198-4647-A2B1-27E5FF5F6F3B}" { "Silverlight" }

		default { '' }
	}
}

<#
.SYNOPSIS 
Converts a GUID identifying a Visual Studio project type to a readable string.

.DESCRIPTION
Converts the value of a Project.Kind GUID to a name.

.NOTES
This cmdlet is used by the StudioShell format files to display color values.

.INPUTS
A GUID string value.

.OUTPUTS
System.String. The project type name, or an empty string if the project type GUID is not recognized.

.EXAMPLE
C:\PS> ls dte:/solution/projects | get-projectType
Windows (C#)
Silverlight

.LINK
http://msdn.microsoft.com/en-us/library/hb23x61k%28v=vs.80%29.aspx
http://www.mztools.com/articles/2008/mz2008017.aspx

#>
