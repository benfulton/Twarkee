using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace TeleTwitter.Lib.Json
{
    public class Helper
    {
        public static T Deserialize<T>(string value)
        {
            return (T)JavaScriptConvert.DeserializeObject(value, typeof(T), new UserConverter(), new StatusConverter());
        }
        public static T Deserialize<T>(string value, params JsonConverter[] converters)
        {
            return (T)JavaScriptConvert.DeserializeObject(value, typeof(T), converters);
        }

        public static string Serialize<T>(T value)
        {
            return JavaScriptConvert.SerializeObject(value, new UserConverter(), new StatusConverter());
        }
        public static string Serialize<T>(T value, params JsonConverter[] converters)
        {
            return JavaScriptConvert.SerializeObject(value, converters);
        }
    }
}
