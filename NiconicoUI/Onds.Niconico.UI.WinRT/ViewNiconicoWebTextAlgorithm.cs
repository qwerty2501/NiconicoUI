using Onds.Niconico.Data.Text;
using Onds.Niconico.UI.Constants;
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

        internal static void UpdateViewText<T>(Span span, object text, Func<string, IReadOnlyList<IReadOnlyNiconicoWebTextSegment>> parseFunc, ViewNiconicoWebTextArgs args)
            where T : class,IReadOnlyNiconicoWebText
        {
            span.Inlines.Clear();
            applyToSpan(span, textToSegments<T>(text, parseFunc), args,text);
        }

        private static IReadOnlyList<IReadOnlyNiconicoWebTextSegment> textToSegments<T>(object text, Func<string, IReadOnlyList<IReadOnlyNiconicoWebTextSegment>> parseFunc)
            where T : class,IReadOnlyNiconicoWebText
        {
            if (text is T)
            {
                return (text as T).Segments;
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

        private static void applyLinkSegmentToInlines<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args,object sourceText)
            where T : IReadOnlyNiconicoWebTextSegment
        {

            applyLinkToInlineBase(inlines, segment, args, sourceText, (link) =>
            {
                link.Inlines.Add(new Run { Text = getViewString(segment, args) });
            });
        }

        private static void applyAnchorLinkToInlines<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args, object sourceText)
            where T : IReadOnlyNiconicoWebTextSegment
        {
            if (args.ViewFriendly)
            {
                applyLinkToInlineBase(inlines, segment, args, sourceText, (link) =>
                {
                    applyToSpan(link, segment.Segments, args, sourceText);
                });
            }
            else
            {
                var span = new Span();
                span.Inlines.Add(new Run { Text = NiconicoWebTextConstant.htmlAnchorStart });
                applyLinkToInlineBase(span.Inlines, segment, args, sourceText, (link) =>
                {
                    link.Inlines.Add(new Run { Text = segment.Url.OriginalString });
                });
                span.Inlines.Add(new Run { Text = NiconicoWebTextConstant.htmlAnchorEnd });
                applyToSpan(span, segment.Segments, args, sourceText);
                inlines.Add(span);
            }

            
        }

        private static void applyLinkToInlineBase<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args, object sourceText, Action<Hyperlink> addAction)
            where T : IReadOnlyNiconicoWebTextSegment
        {
            var link = new Hyperlink();
            link.Click += (hyperLink, clickArgs) =>
            {
                args.ClicAction(sourceText, segment);
            };
            addAction(link);
            inlines.Add(link);
        }

        private static void applySegmentToInlines<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args)
            where T : IReadOnlyNiconicoWebTextSegment
        {
            inlines.Add(new Run { Text = getViewString(segment, args) });
        }

        private static void applyLineBreakSegmentToInlines<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args)
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

        private static void applyBoldElementToInlines<T>(InlineCollection inlines, T segment, ViewNiconicoWebTextArgs args, object sourceText)
            where T : IReadOnlyNiconicoWebTextSegment
        {


            if (args.ViewFriendly)
            {
                var bold = new Bold();
                applyToSpan(bold, segment.Segments, args, sourceText);
                inlines.Add(bold);
            }
            else
            {
                var span = new Span();
                span.Inlines.Add(new Run { Text = NiconicoWebTextConstant.htmlBoldElementStart });
                applyToSpan(span, segment.Segments, args, sourceText);
                span.Inlines.Add(new Run { Text = NiconicoWebTextConstant.htmlBoldElementEnd });
                inlines.Add(span);
            }
        }


        private static void applyToSpan<T>(Span span, IReadOnlyList<T> segments, ViewNiconicoWebTextArgs args,object sourceText)
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
                        applyLinkSegmentToInlines(span.Inlines, segment, args,sourceText);
                        break;

                    case NiconicoWebTextSegmentType.Plain:
                        applySegmentToInlines(span.Inlines, segment, args);
                        break;


                    case NiconicoWebTextSegmentType.LineBreak:
                        applyLineBreakSegmentToInlines(span.Inlines, segment, args);
                        break;

                    case NiconicoWebTextSegmentType.HtmlAnchorElement:
                        applyAnchorLinkToInlines(span.Inlines, segment, args, sourceText);
                        break;

                    case NiconicoWebTextSegmentType.HtmlBoldElement:
                        applyBoldElementToInlines(span.Inlines, segment, args, sourceText);
                        break;

                    default:
                        throw new NotImplementedException("Not Implemented view segment.");
                }
            }


        }
    }
}
