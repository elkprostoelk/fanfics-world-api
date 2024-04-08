namespace FanficsWorld.Common.DTO;

public class FanficCommentDto
{
    public long Id { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public string Text { get; set; } = string.Empty;
    
    public int LikesCount { get; set; }
    
    public bool? CurrentUserReaction { get; set; }
    
    public int DislikesCount { get; set; }
    
    public SimpleUserDto Author { get; set; }
}