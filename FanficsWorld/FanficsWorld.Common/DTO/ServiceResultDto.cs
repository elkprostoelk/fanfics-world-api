namespace FanficsWorld.Common.DTO;

public class ServiceResultDto
{
    public bool IsSuccess { get; set; } = true;
    
    public string? ErrorMessage { get; set; }
}

public class ServiceResultDto<T> : ServiceResultDto where T : new()
{
    public T? Result { get; set; }
}