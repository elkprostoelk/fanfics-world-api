namespace FanficsWorld.DataAccess.Entities;

public class Fandom
{
    public long Id { get; set; }
    
    public string Title { get; set; }
    
    public bool IsDeleted { get; set; }

    public List<Fanfic> Fanfics { get; set; }

    public List<FanficFandom> FanficFandoms { get; set; }
}