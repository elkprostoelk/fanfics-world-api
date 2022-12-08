namespace FanficsWorld.DataAccess.Entities;

public class Feedback
{
    public long Id { get; set; }
    
    public string? Name { get; set; }
    
    public string Email { get; set; }
    
    public string Text { get; set; }
    
    public bool Reviewed { get; set; }
}