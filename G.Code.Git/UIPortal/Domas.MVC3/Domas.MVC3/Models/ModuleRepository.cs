using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Domas.DAP.ADF.License.Module;
using Domas.DAP.ADF.License;

namespace Domas.MVC3.Models
{ 
    public class ModuleRepository : IModuleRepository
    {
        LicenseContext context = new LicenseContext();

        public IQueryable<Module> All
        {
            get { return context.Modules; }
        }

        public IQueryable<Module> AllIncluding(params Expression<Func<Module, object>>[] includeProperties)
        {
            IQueryable<Module> query = context.Modules;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Module Find(System.Guid id)
        {
            return context.Modules.Find(id);
        }

        public void InsertOrUpdate(Module module)
        {
            if (module.ID == default(System.Guid)) {
                // New entity
                //module.ID = Guid.NewGuid();
                context.Modules.Add(module);
            } else {
                // Existing entity
                context.Entry(module).State = EntityState.Modified;
            }
        }

        public void Delete(System.Guid id)
        {
            var module = context.Modules.Find(id);
            context.Modules.Remove(module);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IModuleRepository
    {
        IQueryable<Module> All { get; }
        IQueryable<Module> AllIncluding(params Expression<Func<Module, object>>[] includeProperties);
        Module Find(System.Guid id);
        void InsertOrUpdate(Module module);
        void Delete(System.Guid id);
        void Save();
    }
}