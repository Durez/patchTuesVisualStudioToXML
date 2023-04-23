using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "Identification")]
	public class Identification
	{

		[XmlElement(ElementName = "ID")]
		public DateTime ID { get; set; }

		[XmlElement(ElementName = "Alias")]
		public DateTime Alias { get; set; }
	}

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
		public List<Revision> Revision { get; set; }
	}

	[XmlRoot(ElementName = "DocumentTracking")]
	public class DocumentTracking
	{

		[XmlElement(ElementName = "Identification")]
		public Identification Identification { get; set; }

		[XmlElement(ElementName = "Status")]
		public string Status { get; set; }

		[XmlElement(ElementName = "Version")]
		public double Version { get; set; }

		[XmlElement(ElementName = "RevisionHistory")]
		public RevisionHistory RevisionHistory { get; set; }

		[XmlElement(ElementName = "InitialReleaseDate")]
		public DateTime InitialReleaseDate { get; set; }

		[XmlElement(ElementName = "CurrentReleaseDate")]
		public DateTime CurrentReleaseDate { get; set; }
	}
}
