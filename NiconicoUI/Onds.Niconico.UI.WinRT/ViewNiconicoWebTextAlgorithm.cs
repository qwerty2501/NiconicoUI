#if NETFX_CORE
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Documents;
#endif

using Onds.Niconico.Data.Text;
using Onds.Niconico.UI.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onds.Niconico.UI.Extentions;


namespace Onds.Niconico.UI
{
    internal static class ViewNiconicoWebTextAlgorithm
    {

     


        internal static void UpdateViewText<T>(Span span, object text, Func<string, IReadOnlyList<IReadOnlyNiconicoWebTextSegment>> parseFunc, ViewNiconicoWebTextArgs args)
            where T : class,IReadOnlyNiconicoWebTextSegment,INiconicoText
        {
            span.Inlines.Clear();
            applyToSpan(span, textToSegments<T>(text, parseFunc), args,text);
        }

        private static IReadOnlyList<IReadOnlyNiconicoWebTextSegment> textToSegments<T>(object text, Func<string, IReadOnlyList<IReadOnlyNiconicoWebTextSegment>> parseFunc)
            where T : class,IReadOnlyNiconicoWebTextSegment,INiconicoText
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

        private static string getViewString(IReadOnlyNiconicoWebTextSegment segment, ViewNiconicoWebTextArgs args)
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

        private static void applyLinkSegmentToInlines(InlineCollection inlines, IReadOnlyNiconicoWebTextSegment segment, ViewNiconicoWebTextArgs args,object sourceText)
        {

            applyLinkToInlineBase(inlines, segment, args, sourceText, (link) =>
            {
                link.Inlines.Add(new Run { Text = getViewString(segment, args) });
            });
        }

        private static void applyAnchorLinkToInlines(InlineCollection inlines, IReadOnlyNiconicoWebTextSegment segment, ViewNiconicoWebTextArgs args, object sourceText)
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
                span.Inlines.Add(new Run { Text = NiconicoWebTextStrings.htmlAnchorStart });
                applyLinkToInlineBase(span.Inlines, segment, args, sourceText, (link) =>
                {
                    link.Inlines.Add(new Run { Text = segment.Url.OriginalString });
                });
                span.Inlines.Add(new Run { Text = NiconicoWebTextStrings.htmlAnchorEnd });
                applyToSpan(span, segment.Segments, args, sourceText);
                inlines.Add(span);
            }

            
        }

        private static void applyLinkToInlineBase(InlineCollection inlines, IReadOnlyNiconicoWebTextSegment segment, ViewNiconicoWebTextArgs args, object sourceText, Action<Hyperlink> addAction)
        {
            var link = new Hyperlink();
            link.Click += (hyperLink, clickArgs) =>
            {
                args.ClicAction(sourceText, segment);
            };
            addAction(link);
            inlines.Add(link);
        }

        private static void applySegmentToInlines(InlineCollection inlines, IReadOnlyNiconicoWebTextSegment segment, ViewNiconicoWebTextArgs args)
        {
            inlines.Add(new Run { Text = getViewString(segment, args) });
        }

        private static void applyLineBreakSegmentToInlines(InlineCollection inlines, IReadOnlyNiconicoWebTextSegment segment, ViewNiconicoWebTextArgs args)
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

        private static void applyBoldElementToInlines(InlineCollection inlines, IReadOnlyNiconicoWebTextSegment segment, ViewNiconicoWebTextArgs args, object sourceText)
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
                span.Inlines.Add(new Run { Text = NiconicoWebTextStrings.htmlBoldElementStart });
                applyToSpan(span, segment.Segments, args, sourceText);
                span.Inlines.Add(new Run { Text = NiconicoWebTextStrings.htmlBoldElementEnd });
                inlines.Add(span);
            }
        }

        private static void applyFontElementToInlines( InlineCollection inlines, IReadOnlyNiconicoWebTextSegment segment, ViewNiconicoWebTextArgs args, object sourceText)
        {
            var span = new Span();

            if (args.ViewFriendly)
            {
                if (segment.DecoratedFontElementSize)
                {
                    span.FontSize = segment.CorrectToFontElementSize(args.RootSpan.FontSize);
                }

                if (segment.DecoratedColor)
                {
                    span.Foreground = new SolidColorBrush(segment.GetPlatformColor());
                }



                applyToSpan(span,segment.Segments,args,sourceText);
            }
            else
            {


                span.Inlines.Add(new Run { Text = NiconicoWebTextStrings.CreateFontElementStart(segment) });
                applyToSpan(span, segment.Segments, args, sourceText);
                span.Inlines.Add(new Run { Text = NiconicoWebTextStrings.htmlFontElementEnd });

            }

           

            inlines.Add(span);
        }


        private static void applyToSpan(Span span, IReadOnlyList<IReadOnlyNiconicoWebTextSegment> segments, ViewNiconicoWebTextArgs args,object sourceText)
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

                    case NiconicoWebTextSegmentType.HtmlFontElement:
                        applyFontElementToInlines(span.Inlines, segment, args, sourceText);
                        break;

                    default:
                        throw new NotImplementedException("Not Implemented view segment.");
                }
            }


        }
    }
}
