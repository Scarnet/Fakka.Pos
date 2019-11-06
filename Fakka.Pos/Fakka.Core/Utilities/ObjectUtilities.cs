using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fakka.Core.Utilities
{
    public static class ObjectUtilities
    {
        public static string[] GetModifiedProperties(this object original, object modified)
        {
            var originalType = original.GetType();
            var modifiedType = modified.GetType();

            var originalProperties = originalType.GetProperties();
            var modifiedProperties = modifiedType.GetProperties();

            List<string> changedProperties = new List<string>();

            foreach(var originalProperty in originalProperties)
            {
                var modifiedProperty = modifiedProperties.FirstOrDefault(prop => prop.Name == originalProperty.Name);
                if (modifiedProperty == null)
                    continue;

                var originalValue = originalProperty.GetValue(original);
                var modifiedValue = modifiedProperty.GetValue(modified);

                if (originalValue?.ToString() != modifiedValue?.ToString())
                    changedProperties.Add(originalProperty.Name);
            }

            return changedProperties.ToArray();
        }

        public static void Copy(this object original, object copy)
        {
            var originalType = original.GetType();
            var copyType = copy.GetType();

            var originalProperties = originalType.GetProperties();
            var copyProperties = copyType.GetProperties();


            foreach(var copyProperty in copyProperties)
            {
                var originalProperty = originalProperties.FirstOrDefault(prop => prop.Name == copyProperty.Name);
                if (originalProperty == null)
                    continue;

                copyProperty.SetValue(copy, originalProperty.GetValue(original));
            }
        }
    }
}
