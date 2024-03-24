namespace FanficsWorld.Common.DTO;

public class RegisterUserDto
{
    public required string UserName { get; set; }
    
    public required string Password { get; set; }
    
    public required string Email { get; set; }
    
    public required DateOnly DateOfBirth { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public required string Role { get; set; }
}