using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JisalimuLibrary.Models
{
    class HospitalsRequest
    {
        public string longitude { get; set; }
        public string latitiude { get; set; }
        public string ip_address { get; set; }
        public User user { get; set; }
        public string timestamp { get; set; }
    }
}
