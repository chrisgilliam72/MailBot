using System;
using System.Collections.Generic;

#nullable disable

namespace GMailTest.SQLiteModels
{
    public partial class MailDetail
    {
        public long PkId { get; set; }
        public byte[] MailId { get; set; }
        public byte[] Date { get; set; }
        public byte[] Title { get; set; }
        public byte[] From { get; set; }
    }
}
