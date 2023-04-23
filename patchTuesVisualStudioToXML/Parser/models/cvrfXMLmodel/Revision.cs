using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "Revision")]
	public class Revision
	{

		[XmlElement(ElementName = "Number")]
		public int Number { get; set; }

		[XmlElement(ElementName = "Date")]
		public DateTime Date { get; set; }

		[XmlElement(ElementName = "Description")]
		public string Description { get; set; }
	}

	[XmlRoot(ElementName = "RevisionHistory")]
	public class RevisionHistory
	{

		[XmlElement(ElementName = "Revision")]
		public List<Revision> RevisionsList { get; set; }
	}
}
