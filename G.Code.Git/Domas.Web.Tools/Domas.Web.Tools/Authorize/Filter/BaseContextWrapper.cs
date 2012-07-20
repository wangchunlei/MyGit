using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.Service.Base;
using Domas.Web.Models;
using Domas.Web.Tools.Authorize.Models;

namespace Domas.Web.Tools.Authorize.Filter
{
    public class BaseContextWrapper : BaseContext
    {
        private string userCode;

        public BaseContextWrapper(string usercode)
        {
            userCode = usercode;
        }

        public new IEnumerable<Domas.Service.Base.Role.Role> Roles
        {
            get
            {
                using (var context = new UIComponentContext())
                {
                    var dataResource = context.DataResources.SingleOrDefault(
                        r => r.ResFullName == typeof(Domas.Service.Base.Role.Role).AssemblyQualifiedName);

                    if (dataResource != null)
                    {
                        var rules = dataResource.DataRules.OrderBy(r => r.Seq);
                        foreach (var rule in rules)
                        {
                            if (rule.Rule == "1")
                            {

                            }
                        }
                    }
                }

                return base.Roles;
            }
        }

    }
}
