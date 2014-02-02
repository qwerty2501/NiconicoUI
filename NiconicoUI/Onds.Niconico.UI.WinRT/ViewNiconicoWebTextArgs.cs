#if NETFX_CORE
using Windows.UI.Xaml.Documents;
#endif

using Onds.Niconico.Data.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Onds.Niconico.UI
{
    internal class ViewNiconicoWebTextArgs
    {
        internal ViewNiconicoWebTextArgs(bool viewFriendly, bool enableFontElementSize, Span rootSpan,Action<object, IReadOnlyNiconicoWebTextSegment> clickAction)
        {
            this.ViewFriendly = viewFriendly;
            this.EnableFontElementSize = enableFontElementSize;
            this.ClicAction = clickAction;
            this.RootSpan = rootSpan;
        }

        internal bool ViewFriendly { get; private set; }

        internal bool EnableFontElementSize { get; private set; }

        internal Span RootSpan { get; private set; }

        internal Action<object, IReadOnlyNiconicoWebTextSegment> ClicAction { get; private set; }
    }
}
