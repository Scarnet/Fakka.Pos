using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.JsonConverters
{
    public class JsonStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string strValue = reader.Value?.ToString();

            if (strValue == null)
                return null;

            return JsonConvert.DeserializeObject(strValue, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
           
        }
    }
}
