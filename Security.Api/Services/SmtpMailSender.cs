
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Security.Api;

public class SmtpMailSender : IEmailSender
{
	private readonly SmtpOption _smtpOption;

	public SmtpMailSender(IOptions<SmtpOption> smtpOption)
	{
		_smtpOption = smtpOption.Value;
	}

	public async Task SendEmailAsync(string displayName, string toAddress, string subject, string body)
	{
		using var message = new MailMessage();
		message.To.Add(toAddress);
		message.Subject = subject;
		message.Body = body;
		message.From = new MailAddress(_smtpOption.Username, displayName);
		message.IsBodyHtml = true;

		using var client = new SmtpClient(_smtpOption.Host, _smtpOption.Port);
		client.Credentials = new NetworkCredential(_smtpOption.Username, _smtpOption.Password);
		client.EnableSsl = true;

		await client.SendMailAsync(message);
	}
}
