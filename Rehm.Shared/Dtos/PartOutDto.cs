using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rehm.Shared.Dtos
{
    [XmlRoot("REHM")]
    public class PartOutDto
    {
        [XmlAttribute("s_name")]
        public string s_name { get; set; }

        [XmlAttribute("s_type")]
        public string s_type { get; set; }

        [XmlAttribute("n_id")]
        public int n_id { get; set; }

        [XmlElement("PART_OUT")]
        public PartInOut PartOut { get; set; }
    }

  
}
