using GMailLib;
using GMailTest;
using GMailTest.SQLiteModels;
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

            var mailMessages =GMail.GetMessages();
            foreach (var message in mailMessages)
                Console.WriteLine(message.ToString());
            Console.Read();
        }
    }
}