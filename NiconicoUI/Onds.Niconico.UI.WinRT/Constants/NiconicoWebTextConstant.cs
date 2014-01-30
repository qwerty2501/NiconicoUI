using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onds.Niconico.UI.Constants
{
    internal class NiconicoWebTextConstant
    {
        internal const string htmlElementStart = "<";

        internal const string htmlElementEnd = ">";

        internal const string htmlAnchorStart = htmlElementStart + @"a href=""";

        internal const string htmlAnchorEnd = htmlBoldElementStart + "/a" + htmlElementEnd;

        internal const string htmlBoldElementStart = htmlElementEnd + "b" + htmlElementEnd;

        internal const string htmlBoldElementEnd = htmlBoldElementStart + "/b" + htmlElementEnd;

    }
}
