using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateResource
{
  public  class SupplierCategory_Resource:EntityBase
    {
        public int SysNo { get; set; }
        public int SupplierCategorySysNo { get; set; }

        public string LanguageCode { get; set; }
        public string CategoryName { get; set; }
    }
}
