using System.Net.Mail;
using Vids.Configuration;

namespace Vids.Service
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly INlogger _logger;
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config, INlogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                var emailConfig = _config.GetSection("Email").Get<EmailConfig>();

                SmtpClient client = new SmtpClient("smtp.gmail.com")
                {
                    EnableSsl = true,
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(emailConfig.Account, emailConfig.AppPassword) //"tmncgvytzeuyaaiu"
                };

                client.SendCompleted += SendCompleted;

                MailMessage email = new MailMessage();
                email.From = new MailAddress(emailConfig.Account);
                email.To.Add(new MailAddress(to));
                email.Subject = subject;
                email.Body = body;

                client.SendAsync(email, email);

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                SmtpClient? client = sender as SmtpClient;

                if (client != null)
                {
                    client.SendCompleted -= SendCompleted;
                    client.Dispose();
                }

                MailMessage? email = e.UserState as MailMessage;

                if (email != null)
                {
                    if (e.Error == null)
                    {
                        _logger.Debug(string.Format("{0}| Email [{1}] sent to {2}.", DateTime.Now, email.Subject, GetReceiverMails(email)));
                    }
                    else
                    {
                        _logger.Debug(string.Format("{0}| Failed to sent email [{1}] to {2}. Reason: {3}.", DateTime.Now, email.Subject, GetReceiverMails(email), e.Error.Message));
                    }

                    email.Dispose();

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private string GetReceiverMails(MailMessage email)
        {
            string receiver = string.Empty;

            foreach (var item in email.To)
            {
                if (string.IsNullOrEmpty(receiver))
                {
                    receiver = string.Format("{0}", item.Address);
                }
                else
                {
                    receiver = string.Format("{0}, {1}", receiver, item.Address);
                }
            }

            return receiver;
        }

    }
}