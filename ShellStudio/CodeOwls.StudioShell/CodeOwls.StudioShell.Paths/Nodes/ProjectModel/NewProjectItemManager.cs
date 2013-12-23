/*
   Copyright (c) 2011 Code Owls LLC, All Rights Reserved.

   Licensed under the Microsoft Reciprocal License (Ms-RL) (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.opensource.org/licenses/ms-rl

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.StudioShell.Common.IoC;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Nodes.ProjectModel
{
    /// <summary>
    /// 
    /// </summary>
    static class NewProjectItemManager
    {
        #region Nested type: NewItemDynamicParameters

        public class NewItemDynamicParameters
        {
            [Parameter(ParameterSetName = "FromTemplate", ValueFromPipelineByPropertyName = true)]           
            [Parameter(ParameterSetName = "nameSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "pathSet", ValueFromPipelineByPropertyName = true)]
            [Alias("Language")]
            public string Category { get; set; }

            [Parameter(ParameterSetName = "FromFile", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "nameSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "pathSet", ValueFromPipelineByPropertyName = true)]
            [Alias("FilePath")]
            public string ItemFilePath { get; set; }

            [Parameter(ParameterSetName = "FromProjFile", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "nameSet", ValueFromPipelineByPropertyName = true)]
            [Parameter(ParameterSetName = "pathSet", ValueFromPipelineByPropertyName = true)]
            [Alias("ProjectPath")]
            public string ProjectFilePath { get; set; }
        }

        #endregion

        #region Implementation of INewItem

        public static IEnumerable<string> NewItemTypeNames
        {
            get { return null; }
        }

        public static object NewItemParameters
        {
            get { return new NewItemDynamicParameters(); }
        }

        public static IPathNode NewItem(Project project, ProjectItems items, IContext context, string path, string itemTypeName, object newItemValue)
        {
            if (null == context)
            {
                throw new ArgumentNullException("context");
            }
            if (null == project)
            {
                throw new ArgumentNullException("project");
            }

            var p = context.DynamicParameters as NewItemDynamicParameters;

            DTE2 dte = project.DTE as DTE2;
            Solution2 sln = dte.Solution as Solution2;
            ProjectItem item = null;
            Events2 events2 = dte.Events as Events2;
            var callback = (_dispProjectItemsEvents_ItemAddedEventHandler)((a) => item = a);
            events2.ProjectItemsEvents.ItemAdded += callback;
            try
            {
                if (!String.IsNullOrEmpty(itemTypeName))
                {
                    AddFromItemTypeName(project, items, path, itemTypeName, p, sln);
                }
                else if (!String.IsNullOrEmpty(p.ItemFilePath))
                {
                    AddFromItemFilePath(project, items, p);
                }
                else if (!String.IsNullOrEmpty(p.ProjectFilePath))
                {
                    AddFromProjectFilePath(project, items, p);
                }
            }
            finally
            {
                events2.ProjectItemsEvents.ItemAdded -= callback;
            }
            if (null == item)
            {
                return null;
            }
            var factory = ProjectItemNodeFactory.Create(item);
            return factory.GetNodeValue();
        }

        private static void AddFromProjectFilePath(Project project, ProjectItems items, NewItemDynamicParameters p)
        {
            items.AddFromFile(p.ProjectFilePath);
        }

        private static void AddFromItemFilePath(Project project, ProjectItems items, NewItemDynamicParameters p)
        {
            items.AddFromFileCopy(p.ItemFilePath);
        }

        private static void AddFromItemTypeName(Project project, ProjectItems items, string path, string itemTypeName,
                                                NewItemDynamicParameters p, Solution2 sln)
        {
            if ("folder" == itemTypeName.ToLowerInvariant())
            {
                if (project.Object is SolutionFolder)
                {
                    var folder = project.Object as SolutionFolder;
                    folder.AddSolutionFolder(path);
                }
                else
                {
                    items.AddFolder(path, Constants.vsProjectItemKindPhysicalFolder);
                }
            }
            else
            {
                if (!itemTypeName.ToLowerInvariant().EndsWith(".zip"))
                {
                    itemTypeName += ".zip";
                }

                p.Category = GetSafeCategoryValue(p.Category, project.CodeModel);

                //todo: validate p.Category against available item/project tmps

                if (project.Object is SolutionFolder)
                {
                    SolutionFolder folder = project.Object as SolutionFolder;
                    NewTemplateItemInSolutionFolder(path, itemTypeName, sln, p, folder);
                }
                else
                {
                    var t = sln.GetProjectItemTemplate(itemTypeName, p.Category);
                    items.AddFromTemplate(t, path);
                }
            }
        }

        private static string GetSafeCategoryValue(string category, EnvDTE.CodeModel codeModel)
        {
            const string csharp = "csharp";
            const string vb = "visualbasic";
            const string vcpp = "visualc++";
            const string jsharp = "jsharp";
            var map = new Dictionary<string, string>
                          {
                              {"cs", csharp},
                              {"vb", vb},
                              {"c#", csharp},
                              {"c++", vcpp},
                              {"c+", vcpp},
                              {"cpp", vcpp},
                              {csharp, csharp},
                              {vcpp, vcpp},
                              {vb, vb},
                              {jsharp, jsharp},
                              {CodeModelLanguageConstants.vsCMLanguageCSharp, csharp},
                              {CodeModelLanguageConstants.vsCMLanguageVB, vb},
                              {CodeModelLanguageConstants.vsCMLanguageVC, vcpp},
                              {CodeModelLanguageConstants.vsCMLanguageMC, vcpp},
                              {CodeModelLanguageConstants2.vsCMLanguageJSharp, jsharp},
                          };

            if (String.IsNullOrEmpty(category))
            {
                string language = String.Empty;
                if (null != codeModel && null != codeModel.Language)
                {
                    language = codeModel.Language;
                }
                category = map.ContainsKey(language) ? map[language] : csharp;
            }

            return category;
        }

        private static void NewTemplateItemInSolutionFolder(string path, string itemTypeName, Solution2 sln,
                                                            NewItemDynamicParameters p, SolutionFolder folder)
        {
            var destinationPath = Path.Combine(
                Path.GetDirectoryName(sln.FullName),
                path
                );
            try
            {
                var n = sln.GetProjectTemplate(itemTypeName, p.Category);
                folder.AddFromTemplate(n, destinationPath, path);
            }
            catch (FileNotFoundException)
            {
                var n = sln.GetProjectItemTemplate(itemTypeName, p.Category);
                folder.AddFromTemplate(n, destinationPath, path);
            }
        }
        #endregion

    }
}
