using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "DocumentPublisher")]
	public class DocumentPublisher
	{

		[XmlElement(ElementName = "ContactDetails")]
		public string ContactDetails { get; set; }

		[XmlElement(ElementName = "IssuingAuthority")]
		public string IssuingAuthority { get; set; }

		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}
