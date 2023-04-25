using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.GeneratorXML.models
{

	[XmlRoot(ElementName = "oval_definitions", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5")]
	public class OvalDefinitions
	{
		[XmlElement(ElementName = "generator")]
		public GeneratorTAG generator { get; set; }

		[XmlElement(ElementName = "definitions")]
		public Definitions definitions { get; set; }

		[XmlElement(ElementName = "tests")]
		public Tests tests { get; set; }

		[XmlElement(ElementName = "objects")]
		public Objects objects { get; set; }

		[XmlElement(ElementName = "states")]
		public States states { get; set; }

		[XmlElement(ElementName = "variables")]
		public Variables variables { get; set; }

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
