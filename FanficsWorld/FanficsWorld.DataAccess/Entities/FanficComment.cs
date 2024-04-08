namespace FanficsWorld.DataAccess.Entities;

public class FanficComment
{
    public long Id { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public string Text { get; set; } = string.Empty;

    public List<FanficCommentReaction>? Reactions { get; set; }

    public string AuthorId { get; set; } = string.Empty;

    public User? Author { get; set; }

    public long FanficId;
    
    public Fanfic? Fanfic { get; set; }
}