using System;
using System.Collections.Generic;
using System.Text;

namespace MailScan.SQLiteModels
{
    public class LogMessage
    {
        public int pkID { get; set; }
        public DateTime Date { get; set; }

        public String Message { get; set; }
    }
}
