namespace FanficsWorld.Common.DTO;

public class FandomDTO
{
    public long Id { get; set; }
    
    public string Title { get; set; }

    public ICollection<FanficDTO> Fanfics { get; set; }
}