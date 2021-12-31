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
        public MyUserAdapterModel LoginMyUserAdapterModel { get; set; }
        public MyUserAdapterModel SimulatorMyUserAdapterModel { get; set; }
        private int myUserId;

        public int CurrentMyUserId
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
