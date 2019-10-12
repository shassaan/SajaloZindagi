using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SajaloZindagi.Models
{
    public class Customdetails
    {

        public string ImgName { get; set; }
        public string ImgPath { get; set; }
        public double? ImgPrice { get; set; }
        public int? VId { get; set; }
        public int OId { get; set; }
        public string Oname { get; set; }
        public DateTime? date { get; set; }
        public string description { get; set; }
        public string FuncType { get; set; }
        public string FuncTime { get; set; }
        public string NoOfGuests { get; set; }
    }
}
