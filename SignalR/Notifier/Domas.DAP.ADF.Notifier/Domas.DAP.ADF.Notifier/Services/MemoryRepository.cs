using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.DAP.ADF.Notifier.Infrastructure;
using Domas.DAP.ADF.Notifier.Models;
using Domas.DAP.ADF.NotifierDeploy;

namespace Domas.DAP.ADF.Notifier.Services
{
    public class MemoryRepository : INotifierRepository
    {
        private readonly ICollection<ClientUser> _clients;
        private readonly ICollection<ClientMessage> _messages;
        public MemoryRepository()
        {
            _clients = new SafeCollection<ClientUser>();
            _messages = new SafeCollection<ClientMessage>();
        }

        public IQueryable<ClientUser> ClientUser { get { return _clients.AsQueryable(); } }
        public IQueryable<ClientMessage> Messages { get { return _messages.AsQueryable(); } }
        public void Add(ClientUser client)
        {
            _clients.Add(client);
        }

        public void Add(ClientMessage message)
        {
            _messages.Add(message);
        }

        public void Remove(ClientUser client)
        {
            _clients.Remove(client);
        }

        public void Remove(ClientMessage message)
        {
            _messages.Remove(message);
        }

        public void CommitChanges()
        {
            //memory not impl
            
        }

        public void Dispose()
        {
        }
    }
}
