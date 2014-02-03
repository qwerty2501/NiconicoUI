using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Onds.Niconico.UI.WinRT.Test
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NiconicoWebText.Text = "plainText<b>boldText</b><font size=\"7\">FontSize 7</font><font color=\"red\">redText</font>\r\n<font size=\"1\">FontSize 1</font><i>italic</i><u>underLine</u>\r\n<font size=\"-1\">FontSize-1</font><font size=\"+2\">FontSize+2</font>\r\n" +
                                        "\\r\\n:\r\n\\n:\nLineBreak:<br>" +
                                        "videoId:sm17962764,liveId:lv168019406,communityId:co2268671\r\n" +
                                        "<a href=\"http://www.nicovideo.jp/watch/sm22704573\">aタグはコメントじゃないから無効</a>\r\n"+
                                        "<s>打消し線なくて＼(^o^)／</s>";
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NiconicoWebText.Text = "<b>boldText</b>";

        }
    }
}
