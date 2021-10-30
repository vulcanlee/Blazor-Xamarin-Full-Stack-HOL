using CommonDomain.DataModels;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BAL.Helpers
{
    public static class SmtpHelper
    {
        public static void Initialization(BackendSmtpClientInformation smtpClientInformation)
        {
            // Google Mail 低安全性應用程式存取權
            // https://blog.no2don.com/2021/01/c-gmail-smtp-server-requires-secure.html
            SmtpClient smtp = new SmtpClient
            {
                Host = smtpClientInformation.Host,
                Port = smtpClientInformation.Port,
                EnableSsl = smtpClientInformation.EnableSsl,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(
                    smtpClientInformation.UserName, smtpClientInformation.Password)
            };
            Email.DefaultSender = new SmtpSender(smtp);
        }

        public static async Task<bool> SendSMTP(SendEmailModel sendEmailModel, CancellationToken cancellationToken)
        {
            var email = Email
              .From(sendEmailModel.From)
              .To(sendEmailModel.To)
              .Subject(sendEmailModel.Subject)
              .Body(sendEmailModel.Body, true);

            SendResponse sendResponse = await email.SendAsync(cancellationToken);
            return sendResponse.Successful;
        }
    }
}
