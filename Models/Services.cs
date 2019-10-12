using System;
using System.Collections.Generic;

namespace SajaloZindagi.Models
{
    public partial class Services
    {
        public Services()
        {
            EventServices = new HashSet<EventServices>();
            Simages = new HashSet<Simages>();
        }

        public int SId { get; set; }
        public string SName { get; set; }
        public double? SPrice { get; set; }
        public string SDp { get; set; }
        public bool? SIsAvailable { get; set; }
        public string SDetail { get; set; }

        public ICollection<EventServices> EventServices { get; set; }
        public ICollection<Simages> Simages { get; set; }
    }
}
