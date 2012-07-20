using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MVCScaffolder
{
    public partial class SelectContextFrm : Form
    {
        private Dictionary<string, string> BEList;
        private string projectName;
        private VSLangProj.VSProject CurProj;
        public SelectContextFrm(Dictionary<string, string> beDic, string projectName, VSLangProj.VSProject curProj)
        {
            InitializeComponent();
            this.BEList = beDic;
            this.projectName = projectName;
            this.CurProj = curProj;
        }

        private void SelectContextFrm_Load(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = BEList;

            this.chkBeDllList.DataSource = bs;
            this.chkBeDllList.DisplayMember = "Key";
            this.chkBeDllList.ValueMember = "Value";
        }

        private void btnload_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (KeyValuePair<string, string> item in this.chkBeDllList.CheckedItems)
                {
                    var assembly = Assembly.LoadFrom(item.Value);

                    string assemblyName = item.Key;
                    var byteName = assemblyName.Split('.');
                    var serviceName = byteName[byteName.Length - 2];
                    byteName[byteName.Length - 1] = serviceName + "Context";
                    var contextName = string.Join(".", byteName);

                    var type = assembly.GetType(contextName);
                    var properties = type.GetProperties().Where(c => c.PropertyType.IsGenericType && c.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
                    assembly = null;

                    Dictionary<string, string> classlist = new Dictionary<string, string>();
                    foreach (var info in properties)
                    {
                        classlist.Add(info.Name, info.PropertyType.GetGenericArguments()[0].FullName);
                    }

                    BindingSource bs = new BindingSource();
                    bs.DataSource = classlist;

                    this.chkBeList.DataSource = bs;
                    this.chkBeList.DisplayMember = "Key";
                    this.chkBeList.ValueMember = "Value";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkBeList.Items.Count; i++)
            {
                this.chkBeList.SetItemChecked(i, true);
            }
        }

        private void btnunselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkBeList.Items.Count; i++)
            {
                this.chkBeList.SetItemChecked(i, false);
            }
        }

        private void chkBeDllList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chkBeList.Items.Clear();

            for (int i = 0; i < this.chkBeDllList.Items.Count; i++)
            {
                if (((ListBox)sender).SelectedIndex == i)
                {
                    continue;
                }
                this.chkBeDllList.SetItemChecked(i, false);
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            try
            {
                //add reference
                AddReferences();

                //Area gen
                var area = new Generator.EFRepository.RegisterArea(projectName,
                    ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, string.Empty, string.Empty);
                var areaoutput = area.TransformText();

                WriteFile(areaoutput, new KeyValuePair<string, string>("area", "RegisterArea"));

                foreach (KeyValuePair<string, string> item in this.chkBeList.CheckedItems)
                {
                    //Reporsitory gen
                    var reporsitory = new Generator.EFRepository.Repository(projectName,
                        ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, item.Value, item.Key);

                    if (!reporsitory.HasError)
                    {
                        var output = reporsitory.TransformText();
                        WriteFile(output, item, "Models", "Repository.cs");
                    }

                    //Controller gen
                    var controller = new Generator.Controller.ControllerWithRepository(projectName,
                        ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, item.Value, item.Key);
                    if (!controller.HasError)
                    {
                        var output = controller.TransformText();
                        WriteFile(output, item, "Controllers", "Controller.cs");
                    }

                    //View Index gen
                    var index = new Generator.RazorView.Index(projectName,
                       ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, item.Value, item.Key);
                    if (!index.HasError)
                    {
                        var output = index.TransformText();
                        WriteFile(output, item, @"Views", @"\Index.cshtml");
                    }

                    //View Create gen
                    var create = new Generator.RazorView.Create(projectName,
                       ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, item.Value, item.Key);
                    if (!create.HasError)
                    {
                        var output = create.TransformText();
                        WriteFile(output, item, @"Views", @"\Create.cshtml");
                    }

                    //View Edit gen
                    var edit = new Generator.RazorView.Edit(projectName,
                       ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, item.Value, item.Key);
                    if (!edit.HasError)
                    {
                        var output = edit.TransformText();
                        WriteFile(output, item, @"Views", @"\Edit.cshtml");
                    }

                    //View _CreateOrEdit gen
                    var _CreateOrEdit = new Generator.RazorView._CreateOrEdit(projectName,
                       ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, item.Value, item.Key);
                    if (!_CreateOrEdit.HasError)
                    {
                        var output = _CreateOrEdit.TransformText();
                        WriteFile(output, item, @"Views", @"\_CreateOrEdit.cshtml");
                    }

                    //View Delete gen
                    var delete = new Generator.RazorView.Delete(projectName,
                       ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, item.Value, item.Key);
                    if (!delete.HasError)
                    {
                        var output = delete.TransformText();
                        WriteFile(output, item, @"Views", @"\Delete.cshtml");
                    }

                    //View Delete gen
                    var details = new Generator.RazorView.Details(projectName,
                       ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, item.Value, item.Key);
                    if (!details.HasError)
                    {
                        var output = details.TransformText();
                        WriteFile(output, item, @"Views", @"\Details.cshtml");
                    }

                    //View Search gen
                    var search = new Generator.RazorView.Search(projectName,
                       ((KeyValuePair<string, string>)this.chkBeDllList.SelectedItem).Key, item.Value, item.Key);
                    if (!search.HasError)
                    {
                        var output = search.TransformText();
                        WriteFile(output, item, @"Views", @"\Search.cshtml");
                    }
                }
                foreach (EnvDTE.ProjectItem projectItem in CurProj.Project.ProjectItems.Item("Views").ProjectItems)
                {
                    foreach (EnvDTE.ProjectItem view in projectItem.ProjectItems)
                    {
                        view.Properties.Item("BuildAction").Value = 3;
                    }
                }
                MessageBox.Show("生成完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WriteFile(string output, KeyValuePair<string, string> item, string dir = "", string extentionName = ".cs")
        {
            var rootPath = System.IO.Path.GetDirectoryName(projectName);
            if (!string.IsNullOrEmpty(dir))
            {
                rootPath = System.IO.Path.Combine(rootPath, dir);
            }

            var fileName = item.Value.Split('.').Last();
            var filePath = System.IO.Path.Combine(rootPath, fileName + extentionName);

            if (!System.IO.Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            System.IO.File.WriteAllText(filePath, output,Encoding.UTF8);
            //ToDo: include in project
            CurProj.Project.ProjectItems.AddFromFile(filePath);
        }

        private void AddReferences()
        {
            if (CurProj.References.Find("Domas.Web.Tools") == null)
            {
                CurProj.References.Add(@"D:\View\Trunk\Domas.Component\Web\Domas.Webs.WebTools.dll");
            }
            if (CurProj.References.Find("Domas.Service.CBO.BE") == null)
            {
                CurProj.References.Add(@"D:\View\Trunk\Domas.Component\Service\Domas.Service.CBO.BE.dll");
            }
            if (CurProj.References.Find("Domas.Service.CBO.Deploy") == null)
            {
                CurProj.References.Add(@"D:\View\Trunk\Domas.Component\Service\Domas.Service.CBO.Deploy.dll");
            }
            if (CurProj.References.Find("Domas.Service.Base.BE") == null)
            {
                CurProj.References.Add(@"D:\View\Trunk\Domas.Component\Service\Domas.Service.Base.BE.dll");
            }
            if (CurProj.References.Find("Domas.Service.Base.Deploy") == null)
            {
                CurProj.References.Add(@"D:\View\Trunk\Domas.Component\Service\Domas.Service.Base.Deploy.dll");
            }
            if (CurProj.References.Find("System.ComponentModel.DataAnnotations") == null)
            {
                CurProj.References.Add(@"System.ComponentModel.DataAnnotations");
            }
            if (CurProj.References.Find("System.Web") == null)
            {
                CurProj.References.Add(@"System.Web");
            }
            if (CurProj.References.Find("System.Web.Mvc") == null)
            {
                CurProj.References.Add(@"System.Web.Mvc");
            }
            if (CurProj.References.Find("System.Web.Routing") == null)
            {
                CurProj.References.Add(@"System.Web.Routing");
            }
        }

        private void SelectContextFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
        }
    }
}
