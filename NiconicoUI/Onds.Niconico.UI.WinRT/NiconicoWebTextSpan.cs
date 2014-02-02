#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.Foundation;
using ClickEventHandler = Windows.Foundation.TypedEventHandler<Onds.Niconico.UI.NiconicoWebTextSpan, Onds.Niconico.UI.NiconicoWebTextSegmentClickEventArgs>;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onds.Niconico.Data.Text;




namespace Onds.Niconico.UI
{

    [ContentProperty(Name="Text")]
    public sealed class NiconicoWebTextSpan:Span
    {
        internal static IReadOnlyList<IReadOnlyNiconicoWebTextSegment> OnParseText(string text)
        {
            return NiconicoTextParser.ParseWebText(text).Segments;
           
        }

        public event ClickEventHandler SegmentClick;


        #region InlinesDependencyProperty

        /// <summary>
        /// Gets or sets the value of the <see cref="Inlines" />
        /// property. This is a dependency property.
        /// </summary>
        public new InlineCollection Inlines
        {
            get
            {
                throw new NotSupportedException("Not supported Inlines");
            }
            set
            {
                throw new NotSupportedException("Not supported Inlines");
            }
        }

        /// <summary>
        /// Identifies the <see cref="Inlines" /> dependency property.
        /// </summary>
        internal static readonly DependencyProperty InlinesProperty = DependencyProperty.Register(
            "Inlines",
            typeof(InlineCollection),
            typeof(NiconicoWebTextSpan),
            null);

        #endregion


        #region TextDependencyProperty



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NiconicoWebTextSpan), new PropertyMetadata(string.Empty,textPropertyChanged));



        private static void textPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            ((NiconicoWebTextSpan)d).updateViewText();
        }

       




        #endregion


        #region ViewFriendlyDependencyProperty

        public bool ViewFriendly
        {
            get { return (bool)GetValue(ViewFriendlyProperty); }
            set { SetValue(ViewFriendlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewFriendly.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty ViewFriendlyProperty =
            DependencyProperty.Register("ViewFriendly", typeof(bool), typeof(NiconicoWebTextSpan), new PropertyMetadata(true,viewFriendlyChanged));

        



        private static void viewFriendlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NiconicoWebTextSpan)d).updateViewText();
        }

        #endregion

        #region EnableFontElementSizeProperty

        public bool EnableFontElementSize
        {
            get { return (bool)GetValue(EnableFontElementSizeProperty); }
            set { SetValue(EnableFontElementSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableFontElementSize.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty EnableFontElementSizeProperty =
            DependencyProperty.Register("EnableFontElementSize", typeof(bool), typeof(NiconicoWebTextSpan), new PropertyMetadata(false));

        #endregion


        private void updateViewText()
        {
            var text = this.Text;

            ViewNiconicoWebTextAlgorithm.UpdateViewText<IReadOnlyNiconicoWebText>(this, text, OnParseText, new ViewNiconicoWebTextArgs(this.ViewFriendly,this.EnableFontElementSize,onSegmentClick));
        }

        private void onSegmentClick(object text, IReadOnlyNiconicoWebTextSegment segment)
        {
            if (this.SegmentClick != null)
            {
                this.SegmentClick(this, new NiconicoWebTextSegmentClickEventArgs(text, segment));
            }
        }
    }
}
