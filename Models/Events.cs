using System;
using System.Collections.Generic;

namespace SajaloZindagi.Models
{
    public partial class Events
    {
        public Events()
        {
            SubEvents = new HashSet<SubEvents>();
        }

        public int EId { get; set; }
        public string EName { get; set; }
        public string EDp { get; set; }
        public string EDescriiption { get; set; }

        public ICollection<SubEvents> SubEvents { get; set; }
    }
}
