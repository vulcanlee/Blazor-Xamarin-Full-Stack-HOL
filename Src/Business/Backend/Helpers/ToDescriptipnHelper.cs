using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public static class ToDescriptipnHelper
    {
        public static string MailQueueStatusName(int status)
        {
            if(status == 0)
            {
                return "等待";
            }
            else if (status == 1)
            {
                return "失敗";
            }
            else if (status == 2)
            {
                return "成功";
            }
            else
            {
                return "";
            }
        }
    }
}
