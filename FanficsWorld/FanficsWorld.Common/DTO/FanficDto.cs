using FanficsWorld.Common.Enums;

namespace FanficsWorld.Common.DTO;

public class FanficDto
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
    
    public SimpleUserDto Author { get; set; }

    public List<SimpleUserDto> Coauthors { get; set; }
    
    public List<SimpleFandomDto> Fandoms { get; set; }

    public List<TagDto> Tags { get; set; }
}