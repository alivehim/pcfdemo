using System;

namespace UtilityTools.Data.Domain
{
    public class VLCHistoryItem : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string MediaFile { get; set; }
        public string MessageOwner { get; set; }
        public string ImageUri { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
