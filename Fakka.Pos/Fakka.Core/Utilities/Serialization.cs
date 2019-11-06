using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Fakka.Core.Enums;
using Fakka.Core.Extensions;
using Fakka.Core.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fakka.Core.Utilities
{

    public static class Serialization
    {
        /// <summary>
        ///  Converts Object to JSON string depending on CaseStrategy 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJsonString(object obj)
        {
            var serializerSettings = new JsonSerializerSettings();
            switch (ApplicationManager.Instance.GetServerInfo().DefaultCaseStrategy)
            {
                case CaseStrategy.SnakeCase:
                    serializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                    break;
                case CaseStrategy.CamelCase:
                    serializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                    break;
                case CaseStrategy.Pascal:
                    throw new NotImplementedException();
                default:
                    throw new InvalidEnumArgumentException(typeof(Serialization).Name, (int)ApplicationManager.Instance.GetServerInfo().DefaultCaseStrategy, typeof(CaseStrategy));
            }

            return obj == null ? string.Empty : JsonConvert.SerializeObject(obj, serializerSettings);
        }


        /// <summary>
        /// Converts Object to query params where the key format depends on CaseStrategy 
        /// Used in GET:query or POST:forms
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToKeyValueString(object obj)
        {
            return ObjectToKeyValueString(obj, ApplicationManager.Instance.GetServerInfo().DefaultCaseStrategy);

        }
        public static string ObjectToKeyValueString(object obj, CaseStrategy caseStrategy)
        {
            var keyValuePair = string.Empty;

            if (obj == null) return keyValuePair;

            IEnumerable<string> properties;
            switch (caseStrategy)
            {
                case CaseStrategy.SnakeCase:
                    properties = from p in obj.GetType().GetProperties()
                        where p.GetValue(obj, null) != null
                        select p.Name.ToSnakeCase() + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
                    break;
                case CaseStrategy.CamelCase:
                    properties = from p in obj.GetType().GetProperties()
                        where p.GetValue(obj, null) != null
                        select p.Name.ToCamelCase() + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
                    break;
                case CaseStrategy.Pascal:
                    throw new NotImplementedException();
                default:
                    throw new InvalidEnumArgumentException(typeof(Serialization).Name, (int)ApplicationManager.Instance.GetServerInfo().DefaultCaseStrategy, typeof(CaseStrategy));
            }

            keyValuePair = string.Join("&", properties.ToArray());

            return keyValuePair;

        }
        
    }
}
