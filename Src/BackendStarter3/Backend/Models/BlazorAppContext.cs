using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class BlazorAppContext
    {
        /// <summary>
        /// The IP for the current session
        /// </summary>
        public string CurrentUserIP { get; set; } = "";
    }
}
