namespace FanficsWorld.DataAccess.Entities;

public class FanficCommentReaction
{
    public long Id { get; set; }
    
    public bool IsLike { get; set; }
    
    public long FanficCommentId { get; set; }

    public FanficComment? FanficComment { get; set; }

    public string UserId { get; set; } = string.Empty;

    public User? User { get; set; }
}