using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JisalimuLibrary.Models
{
        public class HospitalList
        {
            public Hospital[] Hospitals { get; set; }
        }
        public class Hospital
        {
            public string hmis { get; set; }
            public string f_name { get; set; }
            public string location { get; set; }
            public string facility_type { get; set; }
            public string agency { get; set; }
            public string sub_location { get; set; }
            public string division { get; set; }
            public Geolocation geolocation { get; set; }
            public string facility_type_name { get; set; }
            public string facility_number { get; set; }
            public string province { get; set; }
            public string district { get; set; }
            public string spatial_reference_method { get; set; }
        }

        public class Geolocation
        {
            public bool needs_recoding { get; set; }
            public string longitude { get; set; }
            public string latitude { get; set; }
        }

}
