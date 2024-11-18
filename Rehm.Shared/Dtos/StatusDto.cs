using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rehm.Shared.Dtos
{
    [XmlRoot("REHM")]
    public class StatusDto
    {


        [XmlAttribute("s_name")]
        public string s_name { get; set; }

        [XmlAttribute("s_type")]
        public string s_type { get; set; }

        [XmlAttribute("n_id")]
        public int n_id { get; set; }

        [XmlElement("STATE")]
        public STATE State { get; set; }

    }
    public class STATE
    {
        [XmlAttribute("n_state")]
        public int n_state { get; set; }

        [XmlAttribute("s_E10")]
        public string s_E10 { get; set; }

        [XmlAttribute("s_state")]
        public string s_state { get; set; }
    }
}
