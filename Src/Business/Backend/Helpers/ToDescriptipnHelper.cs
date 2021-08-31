using BAL.Helpers;
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
            if(status == MagicHelper.MailStatus等待)
            {
                return "等待";
            }
            else if (status == MagicHelper.MailStatus失敗)
            {
                return "失敗";
            }
            else if (status == MagicHelper.MailStatus成功)
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
