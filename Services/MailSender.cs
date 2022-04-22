using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using Newtonsoft.Json.Linq;
using BugTrakerAPI.Model;
using BugTrakerAPI.Services;

namespace BugTrakerAPI.Services
{
    public class MailSender : IMailSender
    {

        async Task IMailSender.SendMail(MailModel mailInfo)
        {
            MailjetClient client = new MailjetClient("b1a483acd9814e3accfc202d95ec5e9f", "c8fbedbd22101dc6df80de6aebda051e"){};
            {
            };
      MailjetRequest request = new MailjetRequest
         {
            Resource = Send.Resource,
         }
            .Property(Send.FromEmail, "namesongaudel@protonmail.com")
            .Property(Send.FromName, "Nameson Gaudel")
            .Property(Send.Subject, mailInfo.subject)
            .Property(Send.TextPart, mailInfo.message)
            .Property(Send.HtmlPart, mailInfo.message)
            .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", mailInfo.toemail}
                 }
                });
         MailjetResponse response = await client.PostAsync(request);
        }
    }
}