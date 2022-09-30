namespace FanficsWorld.Common.DTO;

public class NewFanficDTO
{
    public string Title { get; set; }
    
    public string? Annotation { get; set; }

    public string Text { get; set; }
    
    public string AuthorId { get; set; }
    
    public ICollection<string>? CoauthorIds { get; set; }
}