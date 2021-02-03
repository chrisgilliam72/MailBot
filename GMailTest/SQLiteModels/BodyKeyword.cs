using System;
using System.Collections.Generic;

#nullable disable

namespace MailScan.SQLiteModels
{
    public partial class BodyKeyword
    {
        public long PkId { get; set; }
        public String KeyWord { get; set; }
    }
}
