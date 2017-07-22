using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateResource
{
    public class EntityBase
    {

        public string CompanyCode { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditDateStr { get; set; }
        public string EditUserName { get; set; }
        public int? EditUserSysNo { get; set; }
        public DateTime InDate { get; set; }
        public string InDateStr { get; set; }
        public string InUserName { get; set; }
        public int? InUserSysNo { get; set; }
        public bool IsMyData { get; set; }
    }
}
