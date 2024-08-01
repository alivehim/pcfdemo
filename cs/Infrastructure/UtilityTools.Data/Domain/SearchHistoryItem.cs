using System;

namespace UtilityTools.Data.Domain
{
    public class SearchHistoryItem : BaseEntity
    {
        public string Url { get; set; }
        public string Owner { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LatestUsedTime { get; set; } 
    }
}
