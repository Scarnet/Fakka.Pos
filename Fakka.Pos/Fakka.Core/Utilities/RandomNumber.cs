using System;

namespace Fakka.Core.Utilities
{
    public class RandomNumber
    {
        public static string Generate()
        {
            return (DateTime.Now.Ticks + new Random().Next(1, 100000)).ToString();
        }
    }
}
