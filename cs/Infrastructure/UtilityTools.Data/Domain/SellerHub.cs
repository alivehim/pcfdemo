using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Data.Domain
{
    public class SellerHub : BaseEntity
    {
        public string SellerName { get; set; }
        public string Address { get; set; }

        /// <summary>
        /// the path to save the file
        /// </summary>
        public string StoragePath { get; set; }
    }
}
