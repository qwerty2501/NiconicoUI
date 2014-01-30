#if NETFX_CORE
using Windows.UI;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onds.Niconico.Data.Text;


namespace Onds.Niconico.UI.Extentions
{
    public static class INiconicoWebTextSegmentExtention
    {
        public static Color GetPlatformColor(this IReadOnlyNiconicoWebTextSegment self)
        {
            return self.Color.ToPlatFormColor();
        }

        public static double CorrectToFontElementSize(this IReadOnlyNiconicoWebTextSegment self,double fontSize)
        {
            return (fontSize * self.FontElementSize) / 3;
        }
    }
}
