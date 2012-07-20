using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Domas.DAP.ADF.License.Service;
using Domas.DAP.ADF.License;

namespace Domas.MVC3.Models
{ 
    public class ServiceRepository : IServiceRepository
    {
        Domas.DAP.ADF.License.LicenseContext context = new Domas.DAP.ADF.License.LicenseContext();

        public IQueryable<Service> All
        {
            get { return context.Services; }
        }

        public IQueryable<Service> AllIncluding(params Expression<Func<Service, object>>[] includeProperties)
        {
            IQueryable<Service> query = context.Services;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Service Find(System.Guid id)
        {
            return context.Services.Find(id);
        }

        public void InsertOrUpdate(Service service)
        {
            if (service.ID == default(System.Guid)) {
                // New entity
                //service.ID = Guid.NewGuid();
                context.Services.Add(service);
            } else {
                // Existing entity
                context.Entry(service).State = EntityState.Modified;
            }
        }

        public void Delete(System.Guid id)
        {
            var service = context.Services.Find(id);
            context.Services.Remove(service);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

    public interface IServiceRepository
    {
        IQueryable<Service> All { get; }
        IQueryable<Service> AllIncluding(params Expression<Func<Service, object>>[] includeProperties);
        Service Find(System.Guid id);
        void InsertOrUpdate(Service service);
        void Delete(System.Guid id);
        void Save();
    }
}