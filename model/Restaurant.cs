using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UF3_test.model
{

    public class Restaurant
    {
        public Address address { get; set; }
        public string borough { get; set; }
        public string cuisine { get; set; }
        public List<Grade> grades { get; set; }
        public string name { get; set; }
        public string restaurant_id { get; set; }

    }
}
