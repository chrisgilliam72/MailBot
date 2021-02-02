using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace GMailLib
{
    public class GMail
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json
        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        static string ApplicationName = "Gmail API .NET Quickstart";

        public static List<GMailMessage> GetMessages()
        {
            var messageLst = new List<GMailMessage>();
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");
            var inboxlistRequest = service.Users.Messages.List("me");
            var filters = service.Users.Settings.Filters.List("me");
            var filtersResponse=filters.Execute();
            inboxlistRequest.MaxResults = 500;
            inboxlistRequest.IncludeSpamTrash = false;
            var emailListResponse = inboxlistRequest.Execute();

            if (emailListResponse != null && emailListResponse.Messages != null)
            {
                //loop through each email and get what fields you want...   
                foreach (var email in emailListResponse.Messages)
                {
                    var gMailMsg = new GMailMessage();
                    var emailInfoRequest = service.Users.Messages.Get("me", email.Id);
                    var emailInfoResponse = emailInfoRequest.Execute();
                    if (emailInfoResponse.LabelIds.Contains("SENT"))
                        continue;
                    foreach (var mParts in emailInfoResponse.Payload.Headers)
                    {

                        if (mParts.Name == "Return-Path")
                            gMailMsg.EMail = mParts.Value;
                        if (mParts.Name == "Subject")
                            gMailMsg.Subject = mParts.Value;
                        else if (mParts.Name == "From")
                            gMailMsg.From = mParts.Value;
                        else if (mParts.Name == "Date")
                            gMailMsg.DateReceived = mParts.Value;

                        if (emailInfoResponse.Payload == null || emailInfoResponse.Payload.Parts == null)
                            continue;
                        foreach (MessagePart p in emailInfoResponse.Payload.Parts)
                        {
                            if (p.MimeType == "text/plain")
                            {
                                byte[] data = FromBase64ForUrlString(p.Body.Data);
                                gMailMsg.Body = Encoding.UTF8.GetString(data);
                            }
                               



                            if (p.MimeType == "text/html")
                            {
                                byte[] data = FromBase64ForUrlString(p.Body.Data);
                                gMailMsg.HTML = Encoding.UTF8.GetString(data);
                            }

                        }
                       
                    }

                    messageLst.Add(gMailMsg);
                }


            }

            return messageLst;
        }

        private static byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
            StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
            result.Append(String.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            return Convert.FromBase64String(result.ToString());
        }
    }
}
