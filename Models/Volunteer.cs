using System;
using System.Collections.Generic;

namespace SajaloZindagi.Models
{
    public partial class Volunteer
    {
        public Volunteer()
        {
            Orders = new HashSet<Orders>();
        }

        public int VId { get; set; }
        public string VName { get; set; }
        public string VAddress { get; set; }
        public string VContact { get; set; }
        public string VDp { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
