namespace PIMTool.Payload.Request.Authentication;

public enum Role
{
    Admin,
    Customer
}
public class UserAuthentication
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Role Role { get; set; }

    public UserAuthentication(string username, string password, Role role)
    {
        Username = username;
        Password = password;
        Role = role;
    }
}