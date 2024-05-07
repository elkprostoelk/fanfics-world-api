namespace FanficsWorld.DataAccess.Entities
{
    public class Tag
    {
        public long Id { get; set; }

        public string Name { get; set; }
        
        public bool IsDeleted { get; set; }

        public List<Fanfic> Fanfics { get; set; }

        public List<FanficTag> FanficTags { get; set; }
    }
}
