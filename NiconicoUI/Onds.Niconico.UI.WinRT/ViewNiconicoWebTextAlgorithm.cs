using Onds.Niconico.Data.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace Onds.Niconico.UI
{
    internal static class ViewNiconicoWebTextAlgorithm
    {

        internal static void UpdateViewText(Span span, object text, Func<string, IReadOnlyList<IReadOnlyNiconicoWebTextSegment>> parseFunc, ViewNiconicoWebTextArgs args)
        {
            span.Inlines.Clear();
            applyToSpan(span, textToSegments(text, parseFunc), args);
        }

        private static IReadOnlyList<IReadOnlyNiconicoWebTextSegment> textToSegments(object text, Func<string, IReadOnlyList<IReadOnlyNiconicoWebTextSegment>> parseFunc)
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

        private static string getViewString<T>(T segment, ViewNiconicoWebTextArgs args)
            where T : IReadOnlyNiconicoWebTextSegment
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

        private static void applyLinkSegmentToInline<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args)
            where T : IReadOnlyNiconicoWebTextSegment
        {
            var link = new Hyperlink();
            link.Inlines.Add(new Run { Text = getViewString(segment, args) });
            inlines.Add(link);
        }

        private static void applySegmentToInline<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args)
            where T : IReadOnlyNiconicoWebTextSegment
        {
            inlines.Add(new Run { Text = getViewString(segment, args) });
        }

        private static void applyLineBreakSegmentToInline<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args)
            where T : IReadOnlyNiconicoWebTextSegment
        {
            var viewText = getViewString(segment, args);

            if (viewText == Environment.NewLine)
            {
                inlines.Add(new LineBreak());
            }
            else
            {
                inlines.Add(new Run { Text = viewText });
            }
        }

        private static void applyToSpan<T>(Span span, IReadOnlyList<T> segments, ViewNiconicoWebTextArgs args)
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


                    case NiconicoWebTextSegmentType.LineBreak:
                        applyLineBreakSegmentToInline(span.Inlines, segment, args);
                        break;

                    case NiconicoWebTextSegmentType.HtmlAnchorElement:

                        break;

                    default:
                        throw new NotImplementedException("Not Implemented view segment.");
                }
            }


        }
    }
}
