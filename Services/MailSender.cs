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
            .Property(Send.Subject, "Your email flight plan!")
            .Property(Send.TextPart, "Dear passenger, welcome to Mailjet! May the delivery force be with you!")
            .Property(Send.HtmlPart, "<h3>Dear passenger, welcome to <a href=\"https://www.mailjet.com/\">Mailjet</a>!<br />May the delivery force be with you!")
            .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", "namesongaudel.ng@gmail.com"}
                 }
                });
         MailjetResponse response = await client.PostAsync(request);
        }
    }
}