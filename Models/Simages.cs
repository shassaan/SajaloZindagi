using System;
using System.Collections.Generic;

namespace SajaloZindagi.Models
{
    public partial class Simages
    {
        public Simages()
        {
            Orders = new HashSet<Orders>();
        }

        public int ImgId { get; set; }
        public string ImgPath { get; set; }
        public int? SId { get; set; }
        public bool? IsDeal { get; set; }
        public string ImgDescription { get; set; }
        public double? ImgPrice { get; set; }
        public string ImgName { get; set; }

        public Services S { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
