using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOCS.Process.DynamicRule.Plugin.Models
{
    internal enum Operator
    {
        Euqual = 1,
        DoesNotEqual,
        Contains,
        DoesNotContains,
        BeginsWith,
        DoesNotBeginWith,
        EndsWith,
        DoesNotEnedWith,
        ContainsData,
        DoesNotContainsData,
    }
}
