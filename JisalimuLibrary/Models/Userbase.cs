using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JisalimuLibrary.Models
{
   public class Userbase
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string password { get; set; }
        public string county { get; set; }
        public string password_reset_code { get; set; }
        public string activation_code { get; set; }
        public string role { get; set; }
        public string register_date { get; set; }
    }
}
