using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onds.Niconico.Data.Text;



namespace Onds.Niconico.UI.Extentions
{

#if NETFX_CORE
    using Windows.UI;
#endif


    public static class NiconicoTextColorUtility
    {

        public Color ToPlatFormColor(this NiconicoTextColor color)
        {
            return Color.FromArgb(0xFF, color.R, color.G, color.B);
        }

        public NiconicoTextColor ToNiconicoTextColor(this Color color)
        {
            return new NiconicoTextColor { R = color.R, G = color.G, B = color.B };
        }
    }
}
