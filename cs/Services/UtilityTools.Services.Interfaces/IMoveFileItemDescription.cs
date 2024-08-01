using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityTools.Services.Interfaces
{
    public interface IMoveFileItemDescription
    {
        public string TargetFilePath { get; set; }


        public string StoragePath { get; set; }
    }
}
