using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.Core.Attributes
{
    public class MessageOwnerAttribute: Attribute
    {
        public MessageOwner MessageOwner { get; set; }

        public MessageOwnerAttribute(MessageOwner messageOwner)
        {
            this.MessageOwner = messageOwner;
        }
    }
}
