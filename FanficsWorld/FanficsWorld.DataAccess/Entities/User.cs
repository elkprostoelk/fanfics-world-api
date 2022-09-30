using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.DataAccess.Entities;

public class User : IdentityUser
{
    public ushort Age { get; set; }
    
    public DateTime RegistrationDate { get; set; }

    public ICollection<Fanfic> Fanfics { get; set; } = new HashSet<Fanfic>();

    public ICollection<Fanfic> CoauthoredFanfics { get; set; } = new HashSet<Fanfic>();

    public ICollection<FanficCoauthor> FanficCoauthors { get; set; } = new HashSet<FanficCoauthor>();
}