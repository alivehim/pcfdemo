using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.DependencyInjection
{
    public class MediaGetDescriptionNode
    {
        public MediaGetDescriptionNode(MediaSymbolType symbol, Type handler)
        {
            Symbol = symbol;
            Handler = handler;
        }

        public MediaSymbolType Symbol { get; set; }
        public Type Handler { get; set; }
    }
}
