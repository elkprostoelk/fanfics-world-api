using FanficsWorld.Common.Enums;

namespace FanficsWorld.Common.DTO;

public class FanficDTO
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
    
    public SimpleUserDTO Author { get; set; }

    public ICollection<SimpleUserDTO> Coauthors { get; set; }
}