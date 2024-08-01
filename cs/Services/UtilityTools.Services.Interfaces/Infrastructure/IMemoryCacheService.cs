﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Infrastructure
{
    public interface IMemoryCacheService
    {
        Task GetOrCreateAsync(string cacheKey);
    }
}