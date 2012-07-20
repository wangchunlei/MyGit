using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Domas.DAP.ADF.License.License;
using Domas.DAP.ADF.License;

namespace Domas.MVC3.Models
{ 
    public class ModuleLicenseRepository : IModuleLicenseRepository
    {
        LicenseContext context = new LicenseContext();

        public IQueryable<ModuleLicense> All
        {
            get { return context.ModuleLicenses; }
        }

        public IQueryable<ModuleLicense> AllIncluding(params Expression<Func<ModuleLicense, object>>[] includeProperties)
        {
            IQueryable<ModuleLicense> query = context.ModuleLicenses;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ModuleLicense Find(System.Guid id)
        {
            return context.ModuleLicenses.Find(id);
        }

        public void InsertOrUpdate(ModuleLicense modulelicense)
        {
            if (modulelicense.ID == default(System.Guid)) {
                // New entity
                //modulelicense.ID = Guid.NewGuid();
                context.ModuleLicenses.Add(modulelicense);
            } else {
                // Existing entity
                context.Entry(modulelicense).State = EntityState.Modified;
            }
        }

        public void Delete(System.Guid id)
        {
            var modulelicense = context.ModuleLicenses.Find(id);
            context.ModuleLicenses.Remove(modulelicense);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IModuleLicenseRepository
    {
        IQueryable<ModuleLicense> All { get; }
        IQueryable<ModuleLicense> AllIncluding(params Expression<Func<ModuleLicense, object>>[] includeProperties);
        ModuleLicense Find(System.Guid id);
        void InsertOrUpdate(ModuleLicense modulelicense);
        void Delete(System.Guid id);
        void Save();
    }
}