using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class BackendCustomNLog
    {
        public string LogRootPath { get; set; }
        public string AllLogMessagesFilename { get; set; }
        public string AllWebDetailsLogMessagesFilename { get; set; }
    }
}
