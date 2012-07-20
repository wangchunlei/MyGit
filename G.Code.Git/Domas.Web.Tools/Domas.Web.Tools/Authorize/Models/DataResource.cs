using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domas.Web.Tools.Authorize.Models
{
    public class DataResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid ID { get; set; }
        public System.DateTime? CreatedOn { get; set; }
        public string Createdby { get; set; }
        public System.DateTime? ModifiedOn { get; set; }
        public string Modifiedby { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ResFullName { get; set; }
        public List<DataRule> DataRules { get; set; } 
    }

    public class DataRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid ID { get; set; }
        public Guid DataResourceID { get; set; }
        public DataResource DataResource { get; set; }
        public string Rule { get; set; }
        public string Oper { get; set; }
        public string RuleValue { get; set; }
        public string Operator { get; set; }
        public int Seq { get; set; }
    }
    public class DataMenuOperation
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ActionURL { get; set; }
        public string ControllerFullName { get; set; }
        public string ActionName { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Search { get; set; }
        public bool Menu { get; set; }
        public bool IsAdminSaferAuditor { get; set; }
    }

}
