using BAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public static class HeaderBuilderHelper
    {
        public static string GetTitle(string title)
        {
            return $"Backend - {title}";

        }
    }
}
