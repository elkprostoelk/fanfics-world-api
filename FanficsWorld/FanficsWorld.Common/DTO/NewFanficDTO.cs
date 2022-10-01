using FanficsWorld.Common.Enums;

namespace FanficsWorld.Common.DTO;

public class NewFanficDTO
{
    public string Title { get; set; }
    
    public string? Annotation { get; set; }

    public string Text { get; set; }
    
    public FanficOrigin Origin { get; set; }
    
    public FanficRating Rating { get; set; }
    
    public FanficDirection Direction { get; set; }
    
    public ICollection<string>? CoauthorIds { get; set; }
}