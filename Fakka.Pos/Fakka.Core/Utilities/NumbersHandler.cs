using Fakka.Core.Managers;

namespace Fakka.Core.Utilities
{
    public static class NumbersHandler
    {
        public static string ConvertToArabic(this string input)
        {
            //    if (new string[] {"ar-lb", "ar-SA"}
            //        .Contains(Thread.CurrentThread.CurrentCulture.Name))
            if (ApplicationManager.Instance.GetApplicationInfo().IsRtl)
            {
                return input?.Replace('0', '\u0660')
                        .Replace('1', '\u0661')
                        .Replace('2', '\u0662')
                        .Replace('3', '\u0663')
                        .Replace('4', '\u0664')
                        .Replace('5', '\u0665')
                        .Replace('6', '\u0666')
                        .Replace('7', '\u0667')
                        .Replace('8', '\u0668')
                        .Replace('9', '\u0669')
                    ;
            }
            else return input;
        }
        public static string ConvertToEnglish(this string input)
        {
            //    if (new string[] {"ar-lb", "ar-SA"}
            //        .Contains(Thread.CurrentThread.CurrentCulture.Name))
            if (ApplicationManager.Instance.GetApplicationInfo().IsRtl)
            {
                return input?.Replace('\u0660','0')
                        .Replace('\u0661','1' )
                        .Replace('\u0662','2' )
                        .Replace('\u0663','3' )
                        .Replace('\u0664','4' )
                        .Replace('\u0665','5' )
                        .Replace('\u0666','6' )
                        .Replace('\u0667','7' )
                        .Replace('\u0668','8' )
                        .Replace('\u0669','9' )
                    ;
            }
            else return input;
        }
    }
}
