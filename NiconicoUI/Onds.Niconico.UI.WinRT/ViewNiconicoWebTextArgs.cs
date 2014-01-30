using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onds.Niconico.UI
{
    internal class ViewNiconicoWebTextArgs
    {
        internal ViewNiconicoWebTextArgs(bool viewFriendly)
        {
            this.ViewFriendly = viewFriendly;
        }

        internal bool ViewFriendly { get; private set; }
    }
}
