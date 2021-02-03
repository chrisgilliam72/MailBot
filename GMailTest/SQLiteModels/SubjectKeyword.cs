using System;
using System.Collections.Generic;

#nullable disable

namespace MailScan.SQLiteModels
{
    public partial class SubjectKeyword
    {
        public long PkId { get; set; }
        public String KeyWord { get; set; }
    }
}
