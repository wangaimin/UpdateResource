using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateResource
{
  public  class BidTool_TenderBidStatusItem_Resource:EntityBase
    {
        public int SysNo { get; set; }
        public int BidTool_TenderBidStatusItemSysNo { get; set; }

        public string LanguageCode { get; set; }
        public string NoticeContent { get; set; }

    }
}
