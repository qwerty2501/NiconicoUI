using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onds.Niconico.UI
{
    internal class ViewNiconicoWebTextArgs
    {
        internal ViewNiconicoWebTextArgs(bool viewFriendly,bool enableFontElementSize)
        {
            this.ViewFriendly = viewFriendly;
            this.EnableFontElementSize = enableFontElementSize;
        }

        internal bool ViewFriendly { get; private set; }

        internal bool EnableFontElementSize { get; private set; }
    }
}
