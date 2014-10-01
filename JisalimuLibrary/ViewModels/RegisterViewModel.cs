using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JisalimuLibrary.ViewModels
{
   public class RegisterViewModel
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string password { get; set; }
        public string confrim_password { get; set; }
        public string county { get; set; }
    }
}
