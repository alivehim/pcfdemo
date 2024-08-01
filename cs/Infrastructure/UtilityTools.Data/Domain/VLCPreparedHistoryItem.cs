namespace UtilityTools.Data.Domain
{
    public class VLCPreparedHistoryItem : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImageUri { get; set; }
        public string MediaFile { get; set; }
        public string Duration { get; set; }
        public string MessageOwner { get; set; }
        public string UpdateTime { get; set; }
        public string TrailerUrl { get; set; }
    }
}
