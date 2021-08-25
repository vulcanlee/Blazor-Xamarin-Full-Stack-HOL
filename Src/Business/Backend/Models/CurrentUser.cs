using Backend.AdapterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class CurrentUser
    {
        public MyUserAdapterModel MyUserAdapterModel { get; set; }
        private int myUserId;

        public int MyUserId
        {
            get
            {
                return myUserId;
            }
            set
            {
                myUserId = value;
            }
        }

    }
}
