﻿#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
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
        internal IReadOnlyList<IReadOnlyNiconicoWebTextSegment> OnParseText(string text)
        {
            return NiconicoTextSegmenter.DivideToWebTextSegments(text);
        }


        #region InlinesDependencyProperty
        /// <summary>
        /// The <see cref="Inlines" /> dependency property's name.
        /// </summary>
        internal const string InlinesPropertyName = "Inlines";

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
            InlinesPropertyName,
            typeof(InlineCollection),
            typeof(NiconicoWebTextSpan),
            null);
        #endregion


        #region TextDependencyProperty
        /// <summary>
        /// The <see cref="Text" /> dependency property's name.
        /// </summary>
        internal const string TextPropertyName = "Text";

        /// <summary>
        /// Gets or sets the value of the <see cref="Text" />
        /// property. This is a dependency property.
        /// </summary>
        public object Text
        {
            get
            {
                return (object)GetValue(TextProperty);
            }
            set
            {
                
                
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Text" /> dependency property.
        /// </summary>
        internal static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            TextPropertyName,
            typeof(object),
            typeof(NiconicoWebTextSpan),
            new PropertyMetadata(string.Empty, textPropertyChanged));

        private static void textPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            ((NiconicoWebTextSpan)d).updateViewText();
        }

        private void updateViewText()
        {
            var text = this.Text;
            base.Inlines.Clear();
            ApplyToSpan(this, TextToSegments(text,OnParseText),new ViewNiconicoWebTextArgs(this.ViewFriendly));
        }


        internal static IReadOnlyList<IReadOnlyNiconicoWebTextSegment> TextToSegments(object text,Func<string,IReadOnlyList<IReadOnlyNiconicoWebTextSegment>> parseFunc)
        {
            if (text is IReadOnlyList<IReadOnlyNiconicoWebTextSegment>)
            {
                return text as IReadOnlyList<IReadOnlyNiconicoWebTextSegment>;
            }
            else if (text is string)
            {
                return parseFunc(text as string);
            }
            else
            {
                throw new InvalidOperationException("The Text property is not allow this type.");
            }
        }

        internal static string GetViewString<T>(T segment, ViewNiconicoWebTextArgs args)
            where T:IReadOnlyNiconicoWebTextSegment
        {
            if (args.ViewFriendly)
            {
                return segment.FriendlyText;
            }
            else
            {
                return segment.Text;
            }
        }

        private static void applyLinkSegmentToInline<T>(InlineCollection inlines,T segment,ViewNiconicoWebTextArgs args)
            where T : IReadOnlyNiconicoWebTextSegment
        {
            var link = new Hyperlink();
            link.Inlines.Add(new Run { Text = GetViewString(segment, args) });
            inlines.Add(link);
        }

        private static void applySegmentToInline<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args)
            where T : IReadOnlyNiconicoWebTextSegment
        {
            inlines.Add(new Run { Text = GetViewString(segment, args) });
        }

        internal static void ApplyToSpan<T>(Span span, IReadOnlyList<T> segments,ViewNiconicoWebTextArgs args)
            where T : IReadOnlyNiconicoWebTextSegment
        {
            foreach (var segment in segments)
            {
                switch (segment.SegmentType)
                {
                        //link
                    case NiconicoWebTextSegmentType.ArticleId:
                    case NiconicoWebTextSegmentType.ChanelId:
                    case NiconicoWebTextSegmentType.CommunityId:
                    case NiconicoWebTextSegmentType.LineBreak:
                    case NiconicoWebTextSegmentType.LiveId:
                    case NiconicoWebTextSegmentType.MarketId:
                    case NiconicoWebTextSegmentType.MaterialId:
                    case NiconicoWebTextSegmentType.PictureId:
                    case NiconicoWebTextSegmentType.Url:
                    case NiconicoWebTextSegmentType.UserName:
                    case NiconicoWebTextSegmentType.VideoId:
                        applyLinkSegmentToInline(span.Inlines, segment, args);
                        break;

                    case NiconicoWebTextSegmentType.Plain:
                        applySegmentToInline(span.Inlines, segment, args);
                        break;

                    default:
                        throw new NotImplementedException("Not Implemented view segment.");
                }
            }


        }

        #endregion

        #region ViewFriendly


        /// <summary>
        /// The <see cref="ViewFriendly" /> dependency property's name.
        /// </summary>
        internal const string ViewFriendlyPropertyName = "ViewFriendly";

        /// <summary>
        /// Gets or sets the value of the <see cref="ViewFriendly" />
        /// property. This is a dependency property.
        /// </summary>
        public bool ViewFriendly
        {
            get
            {
                return (bool)GetValue(ViewFriendlyProperty);
            }
            set
            {
                SetValue(ViewFriendlyProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="ViewFriendly" /> dependency property.
        /// </summary>
        internal static readonly DependencyProperty ViewFriendlyProperty = DependencyProperty.Register(
            ViewFriendlyPropertyName,
            typeof(bool),
            typeof(NiconicoWebTextSpan),
            new PropertyMetadata(true, viewFriendlyChanged));

        private static void viewFriendlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NiconicoWebTextSpan)d).updateViewText();
        }

        #endregion
    }
}
