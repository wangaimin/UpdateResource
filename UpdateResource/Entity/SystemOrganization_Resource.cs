using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateResource
{
 public   class SystemOrganization_Resource
    {
        public int SysNo { get; set; }

        /// <summary>
        /// SystemOrganization表SysNo
        /// </summary>
        public int SystemOrganizationSysNo { get; set; }

        public string LanguageCode { get; set; }

        /// <summary>
        /// 组织机构简称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 组织机构全称
        /// </summary>
        public string OrganizationFullName { get; set; }

        public DateTime EditDate { get; set; }
        public string EditUserName { get; set; }
        public int EditUserSysNo { get; set; }

        public DateTime InDate { get; set; }
        public string InUserName { get; set; }
        public int InUserSysNo { get; set; }
    }
}
