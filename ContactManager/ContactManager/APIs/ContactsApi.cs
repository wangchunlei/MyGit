using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using ContactManager.Resources;

namespace ContactManager.APIs
{
    [ServiceContract]
    public class ContactsApi
    {
        [WebInvoke(UriTemplate = "Get/{id}/{name}", Method = "GET")]
        public IQueryable<Contact> Get(int id, string name)
        {
            var contacts = new List<Contact>()
        {
            new Contact {ContactId = 1, Name = "Phil Haack"},
            new Contact {ContactId = 2, Name = "HongMei Ge"},
            new Contact {ContactId = 3, Name = "Glenn Block"},
            new Contact {ContactId = 4, Name = "Howard Dierking"},
            new Contact {ContactId = 5, Name = "Jeff Handley"},
            new Contact {ContactId = 6, Name = "Yavor Georgiev"}
        };
            return contacts.AsQueryable().Where(c => c.ContactId == id || c.Name == name);
        }
        [WebInvoke(UriTemplate = "Filter/{id}/{name}", Method = "PUT")]
        public IQueryable<Contact> Filter(List<Contact> contacts, int id, string name)
        {

            var result = contacts.AsQueryable().Where(c => c.ContactId == id || c.Name == name);

            return result;
        }
    }
}