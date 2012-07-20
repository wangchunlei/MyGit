using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Domas.DAP.ADF.License.License;

namespace Domas.MVC3.Models
{ 
    public class LicenseRepository : ILicenseRepository
    {
        Domas.DAP.ADF.License.LicenseContext context = new Domas.DAP.ADF.License.LicenseContext();
        public IQueryable<License> All
        {
            get { return context.Licenses; }
        }

        public IQueryable<License> AllIncluding(params Expression<Func<License, object>>[] includeProperties)
        {
            IQueryable<License> query = context.Licenses;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public License Find(System.Guid id)
        {
            return context.Licenses.Find(id);
        }

        public void InsertOrUpdate(License license)
        {
            if (license.ID == default(System.Guid)) {
                // New entity
                //license.ID = Guid.NewGuid();
                context.Licenses.Add(license);
            } else {
                // Existing entity
                context.Entry(license).State = EntityState.Modified;
            }
        }

        public void Delete(System.Guid id)
        {
            var license = context.Licenses.Find(id);
            context.Licenses.Remove(license);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface ILicenseRepository
    {
        IQueryable<License> All { get; }
        IQueryable<License> AllIncluding(params Expression<Func<License, object>>[] includeProperties);
        License Find(System.Guid id);
        void InsertOrUpdate(License license);
        void Delete(System.Guid id);
        void Save();
    }
}