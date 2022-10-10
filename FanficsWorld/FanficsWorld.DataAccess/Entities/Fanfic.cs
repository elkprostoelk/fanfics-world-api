using FanficsWorld.Common.Enums;

namespace FanficsWorld.DataAccess.Entities;

public class Fanfic
{
    public long Id { get; set; }
    
    public string Title { get; set; }
    
    public string? Annotation { get; set; }

    public string Text { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime? LastModified { get; set; }
    
    public FanficOrigin Origin { get; set; }
    
    public FanficStatus Status { get; set; }
    
    public FanficRating Rating { get; set; }
    
    public FanficDirection Direction { get; set; }
    
    public string AuthorId { get; set; }
    
    public User? Author { get; set; }

    public ICollection<User> Coauthors { get; set; } = new HashSet<User>();

    public ICollection<FanficCoauthor> FanficCoauthors { get; set; } = new HashSet<FanficCoauthor>();

    public ICollection<Fandom> Fandoms { get; set; } = new HashSet<Fandom>();
    
    public ICollection<FanficFandom> FanficFandoms { get; set; } = new HashSet<FanficFandom>();

    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    public ICollection<FanficTag> FanficTags { get; set; } = new HashSet<FanficTag>();
}