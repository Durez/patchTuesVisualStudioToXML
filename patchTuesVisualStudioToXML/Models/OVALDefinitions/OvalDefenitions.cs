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
		public GeneratorTAG Generator { get; set; }

		[XmlElement(ElementName = "definitions")]
		public Definitions Definitions { get; set; }

		[XmlElement(ElementName = "tests")]
		public Tests Tests { get; set; }

		[XmlElement(ElementName = "objects")]
		public Objects Objects { get; set; }

		[XmlElement(ElementName = "states")]
		public States States { get; set; }

		[XmlElement(ElementName = "variables")]
		public Variables Variables { get; set; }

		[XmlAttribute(AttributeName = "xsi")]
		public string Xsi { get; set; }

		[XmlAttribute(AttributeName = "oval")]
		public string Oval { get; set; }

		[XmlAttribute(AttributeName = "schemaLocation")]
		public string SchemaLocation { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlText]
		public string Text { get; set; }

		
	}
}
