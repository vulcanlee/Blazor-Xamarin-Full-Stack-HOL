using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public enum FlowActionEnum
    {
        Send,
        BackToSend,
        Agree,
        Deny,
    }
}
