using FanficsWorld.Common.Enums;

namespace FanficsWorld.Common.DTO
{
    public class SearchFanficsDto
    {
        public string? SearchByTitle { get; set; }

        public List<long>? FandomIds { get; set; }

        public List<long>? TagIds { get; set; }

        public string? Origins { get; set; }

        public string? Directions { get; set; }

        public string? Statuses { get; set; }

        public string? Ratings { get; set; }

        public SortingValue? SortBy { get; set; }

        public SortingOrder? SortOrder { get; set; }

        public int Page { get; set; } = 1;

        public int ItemsPerPage { get; set; } = 20;
    }
}
