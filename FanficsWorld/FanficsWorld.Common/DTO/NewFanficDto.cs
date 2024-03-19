using FanficsWorld.Common.Enums;

namespace FanficsWorld.Common.DTO;

public class NewFanficDto
{
    public string Title { get; set; }
    
    public string? Annotation { get; set; }

    public string Text { get; set; }
    
    public FanficOrigin Origin { get; set; }
    
    public FanficRating Rating { get; set; }
    
    public FanficDirection Direction { get; set; }
    
    public List<string>? CoauthorIds { get; set; }
    
    public List<long>? FandomIds { get; set; }
    
    public List<long>? TagIds { get; set; }
}