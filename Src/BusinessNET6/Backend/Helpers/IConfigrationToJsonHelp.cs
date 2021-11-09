using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class IConfigrationToJsonHelp
    {
        public static JToken Serialize(IConfiguration config)
        {
            JObject obj = new JObject();
            foreach (var child in config.GetChildren())
            {
                obj.Add(child.Key, Serialize(child));
            }

            if (!obj.HasValues && config is IConfigurationSection section)
                return new JValue(section.Value);

            return obj;
        }
    }
}
