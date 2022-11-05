﻿namespace FanficsWorld.Common.DTO;

public class TagWithFanficsDto
{
    public long Id { get; set; }
    
    public long Name { get; set; }
    
    public ICollection<SimpleFanficDto> Fanfics { get; set; }
}