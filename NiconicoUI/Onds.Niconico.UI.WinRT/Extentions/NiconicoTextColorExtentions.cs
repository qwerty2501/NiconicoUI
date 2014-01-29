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




    public static class NiconicoTextColorExtentions
    {

        public static Color ToPlatFormColor(this NiconicoTextColor color)
        {
            return Color.FromArgb(0xFF, color.R, color.G, color.B);
        }

        public static NiconicoTextColor ToNiconicoTextColor(this Color color)
        {
            return new NiconicoTextColor { R = color.R, G = color.G, B = color.B };
        }
    }
}
