using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SajaloZindagi.Models
{
    public partial class Admin
    {
        [JsonProperty("auname")]
        public string AUname { get; set; }
        [JsonProperty("apassword")]
        public string APassword { get; set; }
        [JsonProperty("afullname")]
        public string AFullName { get; set; }
        [JsonProperty("adp")]
        public string ADp { get; set; }
        [JsonProperty("aemail")]
        public string AEmail { get; set; }
        [JsonProperty("acontact")]
        public string AContact { get; set; }
        [JsonProperty("aaccesstype")]
        public bool? AAccessType { get; set; }
    }
}
