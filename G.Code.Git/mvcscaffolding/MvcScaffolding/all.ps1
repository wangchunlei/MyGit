$proj = Get-Project
if($proj){
    if(!$proj.Object.References.Find("Domas.Web.Tools")){
        $proj.Object.References.Add("D:\View\Trunk\Domas.Component\Tools\Web\Domas.Web.Tools.dll") | Out-Null;
    }
    if(!$proj.Object.References.Find("System.ComponentModel.DataAnnotations")){
        $proj.Object.References.Add("System.ComponentModel.DataAnnotations") | Out-Null;
    }
    if(!$proj.Object.References.Find("System.Web")){
        $proj.Object.References.Add("System.Web") | Out-Null;
    }
    if(!$proj.Object.References.Find("System.Web.Mvc")){
        $proj.Object.References.Add("System.Web.Mvc") | Out-Null;
    }
    if(!$proj.Object.References.Find("System.Web.Routing")){
        $proj.Object.References.Add("System.Web.Routing") | Out-Null;
    }
}

$namespace = (Get-Project).Properties.Item("DefaultNamespace").Value
$names = $namespace.Split('.')
$name=$names[$names.Count-1]

$resolvedScaffolder = Get-Scaffolder "Custom"
$scaffolderFolder = [System.IO.Path]::GetDirectoryName($resolvedScaffolder.Location)
 Add-ProjectItemViaTemplate "RegisterArea" -Template RegisterArea `
 	-Model @{ Namespace = $namespace; Name = $name } `
	-SuccessMessage "Added RegisterArea output at {0}" `
	-TemplateFolders $scaffolderFolder -Project $Project -CodeLanguage "cs" -Force:$true
    
$entityTypes="Domas.DAP.ADF.License.Module.Module","Domas.DAP.ADF.License.Service.Service","Domas.DAP.ADF.License.License.License","Domas.DAP.ADF.License.License.ModuleLicense";
foreach($type in $entityTypes)
{
    Scaffold Controller $type -DbContextType:Domas.DAP.ADF.License.LicenseContext -Repository -Force;
}


foreach($viewItems in (Get-Project).ProjectItems.Item("Views").ProjectItems){
	foreach($item in $viewItems.ProjectItems){
		$item.Properties.Item("BuildAction").Value = [int]3
	}
}