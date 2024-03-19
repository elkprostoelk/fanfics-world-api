namespace FanficsWorld.Common.DTO;

public class ServicePagedResultDto<T> where T: new()
{
    public List<T> PageContent { get; set; }
    
    public long TotalItems { get; set; }
    
    public int CurrentPage { get; set; }
    
    public int PagesCount { get; set; }
    
    public int ItemsPerPage { get; set; }
}