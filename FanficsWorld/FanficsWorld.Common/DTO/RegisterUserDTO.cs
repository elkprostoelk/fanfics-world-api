namespace FanficsWorld.Common.DTO;

public class RegisterUserDTO
{
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }
    
    public ushort Age { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Role { get; set; }
}