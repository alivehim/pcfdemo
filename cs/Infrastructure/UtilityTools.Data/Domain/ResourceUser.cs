namespace UtilityTools.Data.Domain
{
    public class ResourceUser: BaseEntity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsUsed { get; set; }
        public int ReplyCount { get; set; }
        public int Sequence { get; set; }
    }
}
