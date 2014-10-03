using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JisalimuOnline.Models
{
    public class Trigger
    {
        public User user { get; set; }
        public Geolocation location { get; set; }
        public DateTime date { get; set; }
        public string comments { get; set; }
        public string status { get; set; }
    }
}