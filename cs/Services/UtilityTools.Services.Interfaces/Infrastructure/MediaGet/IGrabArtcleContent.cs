﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Infrastructure.MediaGet
{
    public interface IGrabArtcleContent
    {
        Task<string> GetContentAsync(string address);
    }
}
