using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;

namespace Generator.EFRepository
{
    public abstract class RepositoryBase : Base
    {
        protected RepositoryBase(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className)
        {
            
        }
    }

    partial class Repository
    {
        public Repository(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className) { }
    }

    partial class RegisterArea
    {
        public RegisterArea(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className) { }
    }
}
