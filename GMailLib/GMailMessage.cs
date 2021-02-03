using System;
using System.Collections.Generic;
using System.Text;

namespace GMailLib
{
    public class GMailMessage
    {

        public String ID { get; set; }
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

        public int Rank(List<String> keyWords, String rankProperty)
        {
            int rank = 0;
            var lowerCaseProp = rankProperty.ToLower();
            foreach (var keyWord in keyWords)
            {
                if (lowerCaseProp.Contains(keyWord.ToLower()))
                    rank++;
            }

            return rank;
        }
    }
}
