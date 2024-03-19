namespace FanficsWorld.Common.DTO;

public class FandomDto
{
    public long Id { get; set; }
    
    public string Title { get; set; }

    public List<SimpleFanficDto> Fanfics { get; set; }
}