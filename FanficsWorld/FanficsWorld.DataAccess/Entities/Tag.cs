namespace FanficsWorld.DataAccess.Entities
{
    public class Tag
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<Fanfic> Fanfics { get; set; } = new HashSet<Fanfic>();

        public ICollection<FanficTag> FanficTags { get; set; } = new HashSet<FanficTag>();
    }
}
