using System.Linq;

namespace Fakka.Core.Extensions
{
    public static class CaseConversionExtension
    {
        public static string ToSnakeCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
        public static string ToCamelCase(this string str)
        {
            return 
                str.Substring(0, 1).ToLower() +
                str.Substring(1);
        }
    }

}
