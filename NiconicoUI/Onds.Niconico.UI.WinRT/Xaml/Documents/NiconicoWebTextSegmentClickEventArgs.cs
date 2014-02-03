#if NETFX_CORE
using Windows.UI.Xaml;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onds.Niconico.Data.Text;


namespace Onds.Niconico.UI.Xaml.Documents
{
    /// <summary>
    /// Niconico web text segment click evnet args.
    /// </summary>
    public class NiconicoWebTextSegmentClickEventArgs:RoutedEventArgs
    {
        internal NiconicoWebTextSegmentClickEventArgs(object text, IReadOnlyNiconicoWebTextSegment segment)
        {
            this.Text = text;
            this.Segment = segment;
        }

        /// <summary>
        /// Source text.
        /// </summary>
        public object Text { get; private set; }
        
        /// <summary>
        /// Clicked text segment.
        /// </summary>
        public IReadOnlyNiconicoWebTextSegment Segment { get; private set; }
    }
}
