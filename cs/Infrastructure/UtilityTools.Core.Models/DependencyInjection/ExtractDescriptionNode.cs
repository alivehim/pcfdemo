using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.DependencyInjection
{
    public class ExtractDescriptionNode
    {
        public ExtractDescriptionNode(MessageOwner messageOwner, Type handler)
        {
            MessageOwner = messageOwner;
            Handler = handler;
        }

        public MessageOwner MessageOwner { get; set; }

        public Type Handler { get; set; }
    }
}
