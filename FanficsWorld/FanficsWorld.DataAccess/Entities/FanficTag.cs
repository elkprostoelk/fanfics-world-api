namespace FanficsWorld.DataAccess.Entities
{
    public class FanficTag
    {
        public long FanficId { get; set; }

        public long TagId { get; set; }

        public Fanfic? Fanfic { get; set; }

        public Tag? Tag { get; set; }
    }
}