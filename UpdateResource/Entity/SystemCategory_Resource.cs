using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateResource
{
   public class SystemCategory_Resource:EntityBase
    {
        public int SysNo { get; set; }
        public int SystemCategorySysNo { get; set; }

        public string LanguageCode { get; set; }
        public string CategoryName { get; set; }

    }
}
