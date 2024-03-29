﻿namespace Security.Api;

public interface IEmailSender
{
	Task SendEmailAsync(string fromAddress, string toAddress, string subject, string body);
}
