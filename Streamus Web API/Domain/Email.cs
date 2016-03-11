namespace Streamus_Web_API.Domain
{
  public class Email
  {
    public string Subject { get; set; }
    public string Body { get; set; }

    public Email()
    {
      Subject = string.Empty;
      Body = string.Empty;
    }

    public Email(string subject, string body)
      : this()
    {
      Subject = subject;
      Body = body;
    }
  }
}