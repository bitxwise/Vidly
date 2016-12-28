using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vidly.Data
{
    public class NewRentalData
    {
        public int CustomerId { get; set; }

        public List<int> MovieIds { get; set; }
    }
}