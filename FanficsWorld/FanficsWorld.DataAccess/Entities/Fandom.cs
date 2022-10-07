namespace FanficsWorld.DataAccess.Entities;

public class Fandom
{
    public long Id { get; set; }
    
    public string Title { get; set; }

    public ICollection<Fanfic> Fanfics { get; set; } = new HashSet<Fanfic>();

    public ICollection<FanficFandom> FanficFandoms { get; set; } = new HashSet<FanficFandom>();
}