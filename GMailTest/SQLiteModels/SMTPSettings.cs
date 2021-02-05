using System;
using System.Collections.Generic;
using System.Text;

namespace MailScan.SQLiteModels
{
    public class SMTPSettings
    {
        public int pkID { get; set; }
        public String Host { get; set; }
        public int Port { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }

    }
}
