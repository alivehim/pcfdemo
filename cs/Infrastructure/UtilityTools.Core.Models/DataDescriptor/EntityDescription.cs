﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.MessageBus;

namespace UtilityTools.Core.Models.DataDescriptor
{
    public class EntityDescription : BaseResourceMetadata
    {
        public string DisplayName { get; set; }

        public string ObjectId { get; set; }


    }
}
