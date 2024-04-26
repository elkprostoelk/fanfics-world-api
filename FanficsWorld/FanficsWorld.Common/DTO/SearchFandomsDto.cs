namespace FanficsWorld.Common.DTO;

public class SearchFandomsDto
{
    public string? SearchByName { get; set; }

    public int Page { get; set; } = 1;

    public int ItemsPerPage { get; set; } = 5;
}