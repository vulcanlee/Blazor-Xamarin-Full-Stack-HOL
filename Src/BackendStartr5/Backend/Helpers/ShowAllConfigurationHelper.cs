using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class AllConfigurationHelper
    {
        public static void Show(IConfiguration Configuration)
        {
            foreach (var item in Configuration.GetChildren())
            {
                ShowDetail(item, 1);
            }
        }
        public static void ShowDetail(IConfigurationSection ConfigurationSection, int level=0)
        {
            if (ConfigurationSection.Value == null)
            {
                foreach (var item in ConfigurationSection.GetChildren())
                {
                    ShowDetail(item, level + 1);
                }
            }
            else
            {
                string space = string.Concat(Enumerable.Repeat(" ", level*3));
                Console.WriteLine($"{space}" +
                    $"Path:{ConfigurationSection.Path} , Value:{ConfigurationSection.Value}");
            }
        }
    }
}
