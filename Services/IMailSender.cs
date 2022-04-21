using BugTrakerAPI.Model;

namespace BugTrakerAPI.Services
{
    public interface IMailSender
    {
         public Task SendMail(MailModel mailInfo);
    }
}