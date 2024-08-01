using System;

namespace UtilityTools.Data.Domain
{
    public class VLCWebViewHistory : BaseEntity
    {
        public string Address { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
