using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UtilityTools.CEF
{
    public interface IWebLoader : IDisposable
    {
        AutoResetEvent ResouseUrlDetected { get; }
        AutoResetEvent TimeoutEvent { get; }

        //string VideoString { get; set; }
    }

}
