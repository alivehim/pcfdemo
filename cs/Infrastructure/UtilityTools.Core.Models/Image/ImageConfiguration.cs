using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.Image
{
    public class ImageConfiguration
    {

        /// <summary>
        /// Gets or sets a value indicating whether to override height.
        /// </summary>
        /// <value>
        ///   <c>true</c> if height should be overridden; otherwise, <c>false</c>.
        /// </value>
        public bool OverrideHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to override width.
        /// </summary>
        /// <value>
        ///   <c>true</c> if width should be overridden; otherwise, <c>false</c>.
        /// </value>
        public bool OverrideWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Configuration"/> is windowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if windowed; otherwise, <c>false</c>.
        /// </value>
        public bool Windowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool DoublePage { get; set; }
    }
}
