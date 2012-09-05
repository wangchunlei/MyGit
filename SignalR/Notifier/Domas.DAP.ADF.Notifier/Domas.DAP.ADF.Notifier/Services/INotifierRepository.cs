using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.DAP.ADF.Notifier.Models;
using Domas.DAP.ADF.NotifierDeploy;

namespace Domas.DAP.ADF.Notifier.Services
{
    public interface INotifierRepository : IDisposable
    {
        IQueryable<ClientUser> ClientUser { get; }
        IQueryable<ClientMessage> Messages { get; } 
        void Add(ClientUser client);
        void Add(ClientMessage message);
        void Remove(ClientUser client);
        void Remove(ClientMessage message);

        void CommitChanges();
    }
}
