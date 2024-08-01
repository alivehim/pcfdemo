using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.Data.Domain
{
    public class MediaSymbol : BaseEntity
    {
        public string Symbol { get; set; }
        public string Address { get; set; }

        /// <summary>
        /// the path to save the file
        /// </summary>
        public string StoragePath { get; set; }
    }
}
