using System;
using System.Collections.Generic;
using System.Text;

namespace GMailLib
{
    public class GMailMessage
    {
        public String DateReceived { get; set; }
        public String From { get; set; }

        public String Subject { get; set; }

        public String Body { get; set; }

        public String HTML { get; set; }

        public String EMail { get; set; }

        public override String ToString()
        {
            return "From :" + From + " Subject " + " " + Subject + " Date: " + DateReceived + " Emai: " + EMail;
        }

        public String RankSubject(List<String> keyWords)
        {
            foreach (var keyWord in Subject)
            {

            }
        }
    }
}
