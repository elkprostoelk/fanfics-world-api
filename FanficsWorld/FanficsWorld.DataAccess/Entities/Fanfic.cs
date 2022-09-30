namespace FanficsWorld.DataAccess.Entities;

public class Fanfic
{
    public long Id { get; set; }
    
    public string Title { get; set; }
    
    public string? Annotation { get; set; }

    public string Text { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime? LastModified { get; set; }
    
    public string AuthorId { get; set; }
    
    public User? Author { get; set; }

    public ICollection<User> Coauthors { get; set; } = new HashSet<User>();

    public ICollection<FanficCoauthor> FanficCoauthors { get; set; } = new HashSet<FanficCoauthor>();
}