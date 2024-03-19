using FanficsWorld.Common.Enums;

namespace FanficsWorld.Common.DTO;

public class SimpleFanficDto
{
    public long Id { get; set; }
    
    public string Title { get; set; }
    
    public string? Annotation { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime? LastModified { get; set; }
    
    public string Origin { get; set; }
    
    public string Status { get; set; }
    
    public string Rating { get; set; }
    
    public string Direction { get; set; }
    
    public SimpleUserDto Author { get; set; }

    public List<SimpleUserDto> Coauthors { get; set; }
    
    public List<SimpleFandomDto> Fandoms { get; set; }
    
    public List<TagDto> Tags { get; set; }
}