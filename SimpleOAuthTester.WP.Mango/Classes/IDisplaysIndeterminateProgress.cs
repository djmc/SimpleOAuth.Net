using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleOAuthTester.WP.Mango.Classes
{
    internal interface IDisplaysIndeterminateProgress
    {
        bool HasIndeterminateProgress { get; }
        string IndeterminateProgressMessage { get; }
    }
}
