using System;
using System.Collections.Generic;

#nullable disable

namespace MailScan.SQLiteModels
{
    public partial class BlockedAddress
    {
        public long PkId { get; set; }
        public String Emai { get; set; }
    }
}
