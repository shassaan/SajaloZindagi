using System;
using System.Collections.Generic;

namespace SajaloZindagi.Models
{
    public partial class Orders
    {
        public int OId { get; set; }
        public int? ImgId { get; set; }
        public int? VId { get; set; }
        public DateTime? ODate { get; set; }
        public bool? OReadStatus { get; set; }
        public string OPh { get; set; }
        public string OEmail { get; set; }
        public string OName { get; set; }
        public string ODescr { get; set; }
        public string OFuncType { get; set; }
        public string OFuncTime { get; set; }
        public string ONoOfGuests { get; set; }

        public Simages Img { get; set; }
        public Volunteer V { get; set; }
    }
}
