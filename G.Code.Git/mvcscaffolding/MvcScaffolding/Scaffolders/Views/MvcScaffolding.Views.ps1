[T4Scaffolding.Scaffolder(Description = "Adds ASP.NET MVC views for Create/Read/Update/Delete/Index scenarios")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$Controller,
	[string]$ModelType,
	[string]$Area,
	[alias("MasterPage")]$Layout = "",	# If not set, we'll use the default layout
 	[alias("ContentPlaceholderIDs")][string[]]$SectionNames,
	[alias("PrimaryContentPlaceholderID")][string]$PrimarySectionName,
	[switch]$ReferenceScriptLibraries = $false,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[string]$ViewScaffolder = "View",
	[switch]$Force = $false
)

# @("_Layout", "_LayoutList", "_LayoutEdit") | %{
# Add-ProjectItemViaTemplate "Views\Shared\$_.cshtml" -Template $_ `
# 	-Model @{ Namespace = $namespace; ExampleValue = "Hello, world!" } `
#	-SuccessMessage "Added layout  output at {0}" `
#	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force
#}


@("_CreateOrEdit","Create", "Edit", "Delete", "Details", "Index", "Search") | %{
	Scaffold $ViewScaffolder -Controller $Controller -ViewName $_ -ModelType $ModelType -Template $_ -Area $Area -Layout $Layout -SectionNames $SectionNames -PrimarySectionName $PrimarySectionName -ReferenceScriptLibraries:$ReferenceScriptLibraries -Project $Project -CodeLanguage $CodeLanguage -OverrideTemplateFolders $TemplateFolders -Force:$Force -BlockUi
}
