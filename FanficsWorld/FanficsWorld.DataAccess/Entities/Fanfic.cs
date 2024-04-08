using FanficsWorld.Common.Enums;

namespace FanficsWorld.DataAccess.Entities;

public class Fanfic
{
    public long Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string? Annotation { get; set; }

    public string Text { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime LastModified { get; set; }
    
    public FanficOrigin Origin { get; set; }
    
    public FanficStatus Status { get; set; }
    
    public FanficRating Rating { get; set; }
    
    public FanficDirection Direction { get; set; }
    
    public ulong Views { get; set; }

    public string AuthorId { get; set; } = string.Empty;
    
    public User? Author { get; set; }

    public List<User> Coauthors { get; set; } = [];

    public List<FanficCoauthor> FanficCoauthors { get; set; } = [];

    public List<Fandom> Fandoms { get; set; } = [];

    public List<FanficFandom> FanficFandoms { get; set; } = [];

    public List<Tag> Tags { get; set; } = [];

    public List<FanficTag> FanficTags { get; set; } = [];

    public List<FanficComment> Comments { get; set; } = [];
}