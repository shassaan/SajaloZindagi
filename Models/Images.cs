using System;
using System.Collections.Generic;

namespace SajaloZindagi.Models
{
    public partial class Images
    {
        public int ImgId { get; set; }
        public string ImgPath { get; set; }
        public int? SId { get; set; }
        public bool? IsDeal { get; set; }

        public Services S { get; set; }
    }
}
