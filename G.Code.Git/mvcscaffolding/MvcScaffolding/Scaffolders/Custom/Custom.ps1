[T4Scaffolding.Scaffolder(Description = "Add Custom Scaff")][CmdletBinding()]
param(
)

$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$names = $namespace.Split('.')
$name=$names[0]
if($names.Count -eq 2){
	$name = $names[1]
}
$TemplateFolders = (Get-Location).Path
 Add-ProjectItemViaTemplate "RegisterArea" -Template RegisterArea `
 	-Model @{ Namespace = $namespace; Name = $name } `
	-SuccessMessage "Added RegisterArea output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage "cs" -Force:"True"

