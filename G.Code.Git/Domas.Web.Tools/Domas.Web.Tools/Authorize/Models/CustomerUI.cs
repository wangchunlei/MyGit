using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domas.Web.Tools.Authorize.Models
{
    public class CustomerUI
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Code { get; set; }//area_controller_name
        public IList<CustomerUIProperty> CustomerUiProperties { get; set; }
    }

    public class CustomerUIProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public Guid CustomerUIID { get; set; }
        public CustomerUI CustomerUI { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsEnable { get; set; }
        public string PropertyType { get; set; }
        public int Seq { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
    }
}
