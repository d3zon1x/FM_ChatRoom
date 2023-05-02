using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExpansionForCAndS
{
    public static class Extensions
    {
        public static string ToBase64(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] bytes = Encoding.Unicode.GetBytes(json);
            return Encoding.Unicode.GetString(bytes);
        } 
        public static T FromBase64<T>(this string base64Text) 
        {
            byte[] bytes = Encoding.Unicode.GetBytes(base64Text);
            string json = Encoding.Unicode.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
