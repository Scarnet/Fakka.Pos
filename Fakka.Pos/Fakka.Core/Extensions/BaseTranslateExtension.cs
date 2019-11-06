using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Core.Extensions
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class BaseTranslateExtension : IMarkupExtension
    {
        private string _resourceId;
        private Type _assemblyType;

        public BaseTranslateExtension(string resourceId, Type assemblyType)
        {
            _resourceId= resourceId;
            _assemblyType = assemblyType;
        }
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;
            ResourceManager resourceManager = new ResourceManager(_resourceId, _assemblyType.GetTypeInfo().Assembly);

            return resourceManager.GetString(Text, CultureInfo.CurrentCulture);
        }
    }

}