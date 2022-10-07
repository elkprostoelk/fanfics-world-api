namespace FanficsWorld.Common.DTO;

public class FandomDto
{
    public long Id { get; set; }
    
    public string Title { get; set; }

    public ICollection<FanficDto> Fanfics { get; set; }
}