using System.Net;
using System.Net.Mail;
using BL.Configuration;

namespace BL.MailConfirmation
{
    public class VerificationCode
    {
        private Random random = new Random();
        private readonly IConfigurationKey configurationKey;

        public VerificationCode(IConfigurationKey configurationKey)
        {
            this.configurationKey = configurationKey;
        }

        public int MailConfirm(string mail)
        {
           // SmtpClient smtpClient = new SmtpClient(configurationKey.EmailLogin, 587);
            int random_number = random.Next(1000, 9999);
            //string senderEmail = configurationKey.EmailLogin;
            //string senderPassword = configurationKey.EmailPassword;
            //string recipientEmail = mail;
            //string subject = "Mail Confirmation";
            //string body = random_number.ToString();
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
            //smtpClient.EnableSsl = true;
            //var mailMessage = new MailMessage(senderEmail, recipientEmail, subject, body);
           // mailMessage.IsBodyHtml = true;
            //smtpClient.Send(mailMessage);
            return random_number;
        }

        public bool VerificationCodeConfirm(int generatedCode, string insertedCode)
        {
            if (int.TryParse(insertedCode, out int insertedVerificationCode))
            {
                if (insertedVerificationCode != generatedCode)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
