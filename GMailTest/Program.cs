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


//Scaffold-DbContext "Data Source=maildb.db" Microsoft.EntityFrameworkCore.Sqlite -OutputDir SQLiteModels  -Force 

namespace MailScan
{
    class Program
    {

        private static List<BodyKeyword> BodyKeywords { get; set; }
        private static List<SubjectKeyword> SubjectKeywords { get; set; }
        private static SMTPSettings SMTPSettings { get; set; }

        private static System.Timers.Timer _checkTimer;
        private static maildbContext _ctx;

        static private String ComposeEmailBody(GMailMessage mailMessage, int mailRank)
        {
            String bodyMessage;
            bodyMessage = "Dear " + mailMessage.From + "," + Environment.NewLine;
            bodyMessage += Environment.NewLine + "This is an automated response";
            bodyMessage += Environment.NewLine + "Thank you for contacting me.";
            bodyMessage += Environment.NewLine + "This email has been identified as recruitment email with confidence match " + mailRank;
            bodyMessage += Environment.NewLine + "I am not currently in the market for a new position but your email address has been logged and I will notify you once I am ready to start a new job search.";
            bodyMessage += Environment.NewLine + "Yours sincerely ";
            bodyMessage += Environment.NewLine + "Chris Gilliam";
            bodyMessage += Environment.NewLine + Environment.NewLine + "RecruitBot v 1.0";
            return bodyMessage;
        }

        private static async Task LogMessage(String Message)
        {
            var logMessage = new LogMessage()
            {
                Message = Message,
                Date = DateTime.Now,

            };
            _ctx.LogMessages.Add(logMessage);
            await _ctx.SaveChangesAsync();
        }

        public static async Task CheckMessages()
        {
            var savedMailIDs = await _ctx.MailDetails.Select(x => x.MailId).ToListAsync();
            var mailMessages = GMail.GetMessages();

            foreach (var message in mailMessages)
            {
                int subjectRank = message.Rank(SubjectKeywords.Select(x => x.KeyWord).ToList(), message.Subject);
                int bodyRank = message.Body != null ? message.Rank(BodyKeywords.Select(x => x.KeyWord).ToList(), message.Body) : 0;
                if (subjectRank > 0 || bodyRank > 0)
                {
                    Console.WriteLine("Subject Rank: " + subjectRank + " Body Rank:" + bodyRank);
                    Console.WriteLine(message.ToString());
                    var mailhelper = new MailHelper(SMTPSettings);
                    mailhelper.SendAsync("Chrisgilliam1972@gmail.com", new List<string>() { new string("chrisgilliam@vodamail.co.za") },
                        "Re: " + message.Subject
                        , ComposeEmailBody(message, subjectRank + bodyRank));
                    if (!savedMailIDs.Contains(message.ID))
                    {
                        var mailDetail = new MailDetail()
                        {
                            Title = message.Subject,
                            Date = message.DateReceived,
                            Body = message.Body,
                            From = message.From,
                            MailId = message.ID,
                            Rank = subjectRank + bodyRank

                        };
                        _ctx.MailDetails.Add(mailDetail);
                    }

                }

            }

            int results = await _ctx.SaveChangesAsync();
        }

        static async Task Main(string[] args)
        {
            _checkTimer = new System.Timers.Timer(25000);
            _checkTimer.Elapsed += _checkTimer_Elapsed; ;


            var location = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("Working Foler: " + location);

            if (File.Exists(location + @"/maildb.db"))
            {
                _ctx = new maildbContext();

                await LogMessage("System Started");

                BodyKeywords = await _ctx.BodyKeywords.ToListAsync();
                SubjectKeywords = await _ctx.SubjectKeywords.ToListAsync();
                SMTPSettings = await _ctx.SMTPSettings.FirstOrDefaultAsync();
                await CheckMessages();
                _checkTimer.Start();
                Console.ReadLine();
                await LogMessage("System stopped");

            }
            else
                throw new FileNotFoundException(location + @"/maildb.db");

        }

        private static async void _checkTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            await CheckMessages();
        }
    }
}