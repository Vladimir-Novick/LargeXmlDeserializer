using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace example
{
	[XmlRoot(ElementName="Flight")]
	public class Flight {
		[XmlElement(ElementName="PCK_ID")]
		public string PCK_ID { get; set; }
		[XmlElement(ElementName="fl_id1")]
		public string Fl_id1 { get; set; }
		[XmlElement(ElementName="Class1")]
		public string Class1 { get; set; }
		[XmlElement(ElementName="fl_id2")]
		public string Fl_id2 { get; set; }
		[XmlElement(ElementName="Class2")]
		public string Class2 { get; set; }
		[XmlElement(ElementName="IsRestrictActive")]
		public string IsRestrictActive { get; set; }
	}

}
