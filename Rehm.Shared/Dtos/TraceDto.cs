using Rehm.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rehm.Shared.Dtos
{

    [XmlRoot("REHM")]
    public class TraceDto
    {
        [XmlAttribute("s_name")]
        public string EquipmentName { get; set; }

        [XmlAttribute("s_type")]
        public string EquipmentType { get; set; }

        [XmlAttribute("n_id")]
        public int Id { get; set; }

        [XmlElement("TRACE")]
        public Tracedata Trace { get; set; }
    }

    public class Tracedata
    {
        [XmlAttribute("s_serialNo")]
        public string SerialNo { get; set; }

        [XmlAttribute("n_lane")]
        public int Lane { get; set; }

        [XmlAttribute("s_lot")]
        public string Lot { get; set; }

        [XmlAttribute("s_program")]
        public string Program { get; set; }

        [XmlAttribute("s_revision")]
        public string Revision { get; set; }

        [XmlAttribute("n_processingTime")]
        public int ProcessingTime { get; set; }

        [XmlElement("CHANNELS")]
        public Channels Channels { get; set; }

        [XmlElement("SINGLES")]
        public Singles Singles { get; set; }

        [XmlElement("EVENTS")]
        public Events Events { get; set; }
    }

    public class Channels
    {
        [XmlAttribute("n_count")]
        public int Count { get; set; }

        [XmlAttribute("s_state")]
        public string State { get; set; }

        [XmlElement("CHANNEL")]
        public List<Channel> ChannelList { get; set; }
    }

    public class Channel
    {
        [XmlAttribute("s_name")]
        public string Name { get; set; }

        [XmlAttribute("n_maxValue")]
        public int MaxValue { get; set; }

        [XmlAttribute("n_minValue")]
        public int MinValue { get; set; }

        [XmlAttribute("n_toleranceBottom")]
        public int ToleranceBottom { get; set; }

        [XmlAttribute("n_toleranceTop")]
        public int ToleranceTop { get; set; }

        [XmlAttribute("n_setValue")]
        public int SetValue { get; set; }

        [XmlAttribute("s_state")]
        public string State { get; set; }

        [XmlAttribute("s_unit")]
        public string Unit { get; set; }
    }

    public class Singles
    {
        [XmlAttribute("n_count")]
        public int Count { get; set; }

        [XmlElement("SINGLE")]
        public List<Single> SingleList { get; set; }
    }

    public class Single
    {
        [XmlAttribute("s_name")]
        public string Name { get; set; }

        [XmlAttribute("n_minValue")]
        public int MinValue { get; set; }

        [XmlAttribute("n_maxValue")]
        public int MaxValue { get; set; }

        [XmlAttribute("s_unit")]
        public string Unit { get; set; }
    }

    public class Events
    {
        [XmlAttribute("n_count")]
        public int Count { get; set; }

        [XmlElement("EVENT")]
        public List<Event> EventList { get; set; }
    }

    public class Event
    {
        [XmlAttribute("n_no")]
        public int No { get; set; }

        [XmlAttribute("s_description")]
        public string Description { get; set; }

        [XmlAttribute("n_category")]
        public int Category { get; set; }

        [XmlAttribute("s_category")]
        public string CategoryDescription { get; set; }
    }



}
