using System;

namespace UtilityTools.Data.Domain
{
    public class OCRItem : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string SourceUrl { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
