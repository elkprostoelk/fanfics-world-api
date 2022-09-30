namespace FanficsWorld.DataAccess.Entities;

public class FanficCoauthor
{
    public long FanficId { get; set; }
    
    public string CoauthorId { get; set; }
    
    public Fanfic? Fanfic { get; set; }
    
    public User? Coauthor { get; set; }
}