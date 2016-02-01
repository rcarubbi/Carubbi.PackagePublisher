using System.Net.Mail;
using Carubbi.Mailer.Interfaces;
using Carubbi.Utils.Configuration;
using Carubbi.Utils.Data;
using System.Collections.Generic;
using System;
namespace Carubbi.Mailer.Implementation
{
    public class SmtpSender : IMailSender
    {
        public string Username { get; set; }
        public string Password { get; set; }

        private bool? _useSSL;
        private string _host;
        private int? _portNumber;

        public bool UseSSL
        {
            get
            {
                if (!_useSSL.HasValue)
                {
                    _useSSL = config["EnableSSLSMTP"].To<bool>(false);
                }

                return _useSSL.Value;
            }
            set
            {
                _useSSL = value;
            }
        }


        public string Host
        {
            get
            {
                if (string.IsNullOrEmpty(_host))
                {
                    _host = config["HostSMTP"];
                }
                return _host;
            }
            set
            {
                _host = value;
            }
        }

        public int PortNumber
        {
            get
            {
                if (!_portNumber.HasValue)
                {
                    _portNumber = config["PortNumberSMTP"].To<int>(UseSSL ? DEFAULT_SSL_SMTP_PORT : DEFAULT_NON_SSL_SMTP_PORT);
                }
                return _portNumber.Value;
            }
            set
            {
                _portNumber = value;
            }
        }


        private AppSettings config;
        public SmtpSender()
        {
            config = new AppSettings("CarubbiMailer");
        }
        #region IMailSender Members


        private const int DEFAULT_SSL_SMTP_PORT = 465;
        private const int DEFAULT_NON_SSL_SMTP_PORT = 25;

        public void Send(System.Net.Mail.MailMessage message)
        {

            SmtpClient smtp = new SmtpClient
            {
                Host = this.Host,
                EnableSsl = this.UseSSL,
                Port = this.PortNumber,
                Credentials = new System.Net.NetworkCredential(Username, Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };

            smtp.Send(message);
            smtp = null;

        }

        #endregion



        public void Dispose()
        {

        }
    }
}
