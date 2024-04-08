using Microsoft.AspNetCore.Identity;

namespace FanficsWorld.DataAccess.Entities;

public class User : IdentityUser
{
    public DateOnly DateOfBirth { get; set; }
    
    public DateTime RegistrationDate { get; set; }

    public List<Fanfic> Fanfics { get; set; }

    public List<Fanfic> CoauthoredFanfics { get; set; }

    public List<FanficCoauthor> FanficCoauthors { get; set; }

    public List<FanficComment> FanficComments { get; set; }

    public List<FanficCommentReaction> FanficCommentReactions { get; set; }
}