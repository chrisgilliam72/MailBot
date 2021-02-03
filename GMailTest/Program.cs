using GMailLib;
using MailScan;
using MailScan.SQLiteModels;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GmailQuickstart
{
    class Program
    {
        static async Task<List<BodyKeyword>> GetBodyKeywords()
        {
            var bodyKeywordsLst = new List<BodyKeyword>() ;
            var ctx = new maildbContext();
            using (ctx)
            {
                bodyKeywordsLst = await  ctx.BodyKeywords.ToListAsync();
            }

            return bodyKeywordsLst;
        }


        static async Task<List<SubjectKeyword>> GetSubjectKeywords()
        {
            var bodyKeywordsLst = new List<SubjectKeyword>();
            var ctx = new maildbContext();
            using (ctx)
            {
                bodyKeywordsLst = await ctx.SubjectKeywords.ToListAsync();
            }

            return bodyKeywordsLst;
        }

    //Scaffold-DbContext "Data Source=maildb.db" Microsoft.EntityFrameworkCore.Sqlite -OutputDir SQLiteModels  -Force 

    static async Task Main(string[] args)
        {

            var subjectKeyWords = await GetSubjectKeywords();
            var bodyKeywords = await GetBodyKeywords();
            var ctx = new maildbContext();
            using (ctx)
            {

                var mailMessages = GMail.GetMessages();
                foreach (var message in mailMessages)
                {
                    int subjectRank = message.Rank(subjectKeyWords.Select(x => x.KeyWord).ToList(), message.Subject);
                    int bodyRank = message.Body != null ? message.Rank(bodyKeywords.Select(x => x.KeyWord).ToList(), message.Body) : 0;
                    if (subjectRank > 0 || bodyRank > 0)
                    {
                        Console.WriteLine("Subject Rank: " + subjectRank + " Body Rank:" + bodyRank);
                        Console.WriteLine(message.ToString());

                        var mailDetail = new MailDetail()
                        {
                            Title = message.Subject,
                            Date = message.DateReceived,
                            Body = message.Body,
                            From = message.From,
                            MailId = message.ID,
                            Rank = subjectRank + bodyRank

                        };
                        ctx.MailDetails.Add(mailDetail);
                    }

                }

                int results =await ctx.SaveChangesAsync();
            }
       

            Console.Read();
        }
    }
}