using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public class HelperInfo
    {
        public static T ConvertToType<T>(object value)
        {
            var jsonData = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        public static List<T> ConvertListToType<T>(object value)
        {
            var jsonData = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<List<T>>(jsonData);
        }
    }
}
