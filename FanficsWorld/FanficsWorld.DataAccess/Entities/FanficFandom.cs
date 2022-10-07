namespace FanficsWorld.DataAccess.Entities;

public class FanficFandom
{
    public long FanficId { get; set; }
    
    public long FandomId { get; set; }
    
    public Fanfic? Fanfic { get; set; }
    
    public Fandom? Fandom { get; set; }
}