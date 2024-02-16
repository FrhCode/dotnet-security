namespace Security.Api;

public class SmtpOption
{
	public string Host { get; set; } = String.Empty;
	public string Username { get; set; } = String.Empty;
	public string Password { get; set; } = String.Empty;
	public int Port { get; set; }
	// public string From { get; set; } = String.Empty;
	// public bool UseSsl { get; set; }
}
