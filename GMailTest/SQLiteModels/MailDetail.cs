using System;
using System.Collections.Generic;

#nullable disable

namespace MailScan.SQLiteModels
{
    public partial class MailDetail
    {
        public long PkId { get; set; }
        public String MailId { get; set; }
        public String Date { get; set; }
        public String Title { get; set; }
        public String From { get; set; }
        public long Rank { get; set; }
        public String Body { get; set; }
    }
}
