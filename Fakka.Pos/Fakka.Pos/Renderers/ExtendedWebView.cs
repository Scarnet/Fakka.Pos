using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fakka.Pos.Renderers
{
    public class ExtendedWebView : WebView
    {
        public Action ClearAllCache { get; set; }
    }
}
