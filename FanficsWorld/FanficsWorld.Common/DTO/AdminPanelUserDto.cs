namespace FanficsWorld.Common.DTO;

public class AdminPanelUserDto
{
    public string Id { get; set; }
    
    public string UserName { get; set; }
    
    public DateTime RegistrationDate { get; set; }
    
    public DateOnly DateOfBirth { get; set; }
    
    public string Email { get; set; }
    
    public int FanficsCount { get; set; }
    
    public int CoauthoredFanficsCount { get; set; }
}