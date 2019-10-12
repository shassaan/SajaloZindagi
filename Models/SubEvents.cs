using System;
using System.Collections.Generic;

namespace SajaloZindagi.Models
{
    public partial class SubEvents
    {
        public SubEvents()
        {
            EventServices = new HashSet<EventServices>();
        }

        public int SeId { get; set; }
        public string SeName { get; set; }
        public string SeDp { get; set; }
        public int? EId { get; set; }

        public Events E { get; set; }
        public ICollection<EventServices> EventServices { get; set; }
    }
}
