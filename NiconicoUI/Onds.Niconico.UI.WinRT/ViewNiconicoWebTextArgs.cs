﻿using Onds.Niconico.Data.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onds.Niconico.UI
{
    internal class ViewNiconicoWebTextArgs
    {
        internal ViewNiconicoWebTextArgs(bool viewFriendly,bool enableFontElementSize,Action<object, IReadOnlyNiconicoWebTextSegment> clickAction)
        {
            this.ViewFriendly = viewFriendly;
            this.EnableFontElementSize = enableFontElementSize;
            this.ClicAction = clickAction;
        }

        internal bool ViewFriendly { get; private set; }

        internal bool EnableFontElementSize { get; private set; }

        internal Action<object, IReadOnlyNiconicoWebTextSegment> ClicAction { get; private set; }
    }
}
