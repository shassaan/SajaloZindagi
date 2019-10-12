using System;
using System.Collections.Generic;

namespace SajaloZindagi.Models
{
    public partial class EventServices
    {
        public int EsId { get; set; }
        public int? SeId { get; set; }
        public int? SId { get; set; }

        public Services S { get; set; }
        public SubEvents Se { get; set; }
    }
}
