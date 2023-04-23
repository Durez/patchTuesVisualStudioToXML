using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "Note")]
	public class Note
	{

		[XmlAttribute(AttributeName = "Title")]
		public string Title { get; set; }

		[XmlAttribute(AttributeName = "Audience")]
		public string Audience { get; set; }

		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }

		[XmlAttribute(AttributeName = "Ordinal")]
		public string Ordinal { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

}
