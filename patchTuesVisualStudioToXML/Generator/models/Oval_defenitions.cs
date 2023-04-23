using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Generator.models
{

	[System.SerializableAttribute()]
	[XmlRoot(ElementName = "oval_definitions", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5")]
	public class oval_definitions
	{

		[XmlElement(ElementName = "generator")]
		public Generator generator { get; set; }

		[XmlElement(ElementName = "variables")]
		public variables variables { get; set; }

		[XmlElement(ElementName = "states")]
		public states states { get; set; }

		[XmlElement(ElementName = "objects")]
		public objects objects { get; set; }

		[XmlElement(ElementName = "definitions")]
		public definitions definitions { get; set; }

		[XmlElement(ElementName = "tests")]
		public tests tests { get; set; }

		[XmlAttribute(AttributeName = "xsi")]
		public string xsi { get; set; }

		[XmlAttribute(AttributeName = "oval")]
		public string oval { get; set; }

		[XmlAttribute(AttributeName = "schemaLocation")]
		public string schemaLocation { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlText]
		public string text { get; set; }

		
	}
}
