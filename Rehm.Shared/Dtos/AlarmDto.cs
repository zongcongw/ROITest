using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rehm.Shared.Dtos
{
    [XmlRoot("REHM")]
    public class AlarmDto
    {
        [XmlAttribute("s_name")]
        public string s_name { get; set; }

        [XmlAttribute("s_type")]
        public string s_type { get; set; }

        [XmlAttribute("n_id")]
        public int n_id { get; set; }

        [XmlElement("EVENT")]
        public events Event { get; set; }
    }

    public class events
    {
        [XmlAttribute("n_no")]
        public string n_no { get; set; }
        [XmlAttribute("b_come")]
        public string b_come { get; set;}
        [XmlAttribute("s_description")]
        public string s_description { get; set;}
        [XmlAttribute("n_category")]
        public string n_category { get; set;}
        [XmlAttribute("s_category")]
        public string s_category { get; set;}
    }
}
