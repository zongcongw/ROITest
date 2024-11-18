using Rehm.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rehm.Shared.Dtos
{

    [XmlRoot("REHM")]
    public class InterlockDto
    {
        [XmlAttribute("s_name")]
        public string s_name { get; set; }

        [XmlAttribute("s_type")]
        public string s_type { get; set; }

        [XmlAttribute("n_id")]
        public int n_id { get; set; }

        [XmlElement("PROCESS_INTERLOCK_REQ")]
        public PROCESS_INTERLOCK_REQ PartIn { get; set; }
    }

    public class PROCESS_INTERLOCK_REQ
    {
        [XmlAttribute("s_serialNo")]
        public string Barcode { get; set; }
        [XmlAttribute("n_lane")]
        public string lane { get; set; }

        [XmlAttribute("s_program")]
        public string program { get; set; }

        [XmlAttribute("s_revision")]
        public string revision { get; set; }
    }


    [XmlRoot("REHM")]
    public class InterlockRespDto
    {

        [XmlElement("PROCESS_INTERLOCK_RESP")]
        public PROCESS_INTERLOCK_RESP EntryCheckResp { get; set; }
    }

    public class PROCESS_INTERLOCK_RESP
    {
        [XmlAttribute("s_serialNo")]
        public string barcode { get; set; }

        [XmlAttribute("s_product")]
        public string product { get; set; } 
        
        [XmlAttribute("s_productRevision")]
        public string productRevision { get; set; }

        [XmlAttribute("s_program")]
        public string program { get; set; }

        [XmlAttribute("s_programRevision")]
        public string programRevision { get; set; }

        [XmlAttribute("s_lot")]
        public string lot { get; set; }

        [XmlAttribute("b_result")]
        public string result { get; set; }//

        [XmlAttribute("s_error")]
        public string error { get; set; }//s_error
    }
}
