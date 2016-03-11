namespace Streamus_Web_API.Dto
{
  public class ContactDto
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }

    public ContactDto()
    {
      Name = string.Empty;
      Email = string.Empty;
      Message = string.Empty;
    }

    public ContactDto(string name, string email, string message)
      : this()
    {
      Name = name;
      Email = email;
      Message = message;
    }
  }
}