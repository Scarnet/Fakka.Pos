using Fakka.Core.Extensions;
using Xamarin.Forms;

namespace Fakka.Pos.Resources
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : BaseTranslateExtension
    {
        public TranslateExtension() : base("Fakka.Pos.Resources.UiResources", typeof(TranslateExtension))
        {

        }
    }

}