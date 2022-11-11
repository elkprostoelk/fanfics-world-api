using FanficsWorld.Common.Enums;

namespace FanficsWorld.Common.DTO;

public class MinifiedFanficDto
{
    public long Id { get; set; }
    
    public FanficStatus FanficStatus { get; set; }
    
    public DateTime? LastModified { get; set; }
}