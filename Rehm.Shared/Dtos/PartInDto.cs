using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rehm.Shared.Dtos
{
    [XmlRoot("REHM")]
    public class PartInDto
    {
        [XmlAttribute("s_name")]
        public string s_name { get; set; }

        [XmlAttribute("s_type")]
        public string s_type { get; set; }

        [XmlAttribute("n_id")]
        public int n_id { get; set; }

        [XmlElement("PART_IN")]
        public PartInOut PartIn { get; set; }
    }
    

    public class PartInOut
    {
        [XmlAttribute("s_serialNo")]
        public string Barcode { get; set; }
        [XmlAttribute("n_lane")]
        public string lane { get; set; }

        [XmlAttribute("s_lot")]
        public string lot { get; set; }
    }
}
