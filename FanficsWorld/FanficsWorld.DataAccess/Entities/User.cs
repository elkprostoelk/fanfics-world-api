using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.DataAccess.Entities;

public class User : IdentityUser
{
    public ushort Age { get; set; }
    
    public DateTime RegistrationDate { get; set; }

    public List<Fanfic> Fanfics { get; set; }

    public List<Fanfic> CoauthoredFanfics { get; set; }

    public List<FanficCoauthor> FanficCoauthors { get; set; }
}