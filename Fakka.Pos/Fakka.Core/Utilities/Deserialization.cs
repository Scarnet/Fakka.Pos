using System;
using System.ComponentModel;
using System.IO;
using Fakka.Core.Enums;
using Fakka.Core.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fakka.Core.Utilities
{
    public static class Deserialization
    {
        /// <summary>
        /// Converts JSON string to Object depending on CaseStrategy 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <param name="caseStrategy"></param>
        /// <returns></returns>
        public static T JsonStringToObject<T>(string jsonString, CaseStrategy? caseStrategy)
        {
            caseStrategy = caseStrategy ?? ApplicationManager.Instance.GetServerInfo().DefaultCaseStrategy;

            var serializerSettings = new JsonSerializerSettings();
            switch (caseStrategy)
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
                    throw new InvalidEnumArgumentException(typeof(Deserialization).Name, (int)ApplicationManager.Instance.GetServerInfo().DefaultCaseStrategy, typeof(CaseStrategy));
            }

            var result = JsonConvert.DeserializeObject<T>(jsonString, serializerSettings);
            return result;
        }

        public static T JsonFileToObject<T>(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                var jsonString = r.ReadToEnd();
                return JsonStringToObject<T>(jsonString, null);
            }
        }

    }
}