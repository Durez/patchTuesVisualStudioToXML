using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "cvrfdoc")]
	public class Cvrfdoc
	{

		[XmlElement(ElementName = "DocumentTitle")]
		public string DocumentTitle { get; set; }

		[XmlElement(ElementName = "DocumentType")]
		public string DocumentType { get; set; }

		[XmlElement(ElementName = "DocumentPublisher")]
		public DocumentPublisher DocumentPublisher { get; set; }

		[XmlElement(ElementName = "DocumentTracking")]
		public DocumentTracking DocumentTracking { get; set; }

		[XmlElement(ElementName = "DocumentNotes")]
		public DocumentNotes DocumentNotes { get; set; }

		[XmlElement(ElementName = "ProductTree")]
		public ProductTree ProductTree { get; set; }

		[XmlElement(ElementName = "Vulnerability")]
		public List<Vulnerability> Vulnerability { get; set; }

		[XmlAttribute(AttributeName = "cpe-lang")]
		public string CpeLang { get; set; }

		[XmlAttribute(AttributeName = "scap-core")]
		public string ScapCore { get; set; }

		[XmlAttribute(AttributeName = "cvrf-common")]
		public string CvrfCommon { get; set; }

		[XmlAttribute(AttributeName = "dc")]
		public string Dc { get; set; }

		[XmlAttribute(AttributeName = "prod")]
		public string Prod { get; set; }

		[XmlAttribute(AttributeName = "cvssv2")]
		public string Cvssv2 { get; set; }

		[XmlAttribute(AttributeName = "vuln")]
		public string Vuln { get; set; }

		[XmlAttribute(AttributeName = "sch")]
		public string Sch { get; set; }

		[XmlAttribute(AttributeName = "cvrf")]
		public string Cvrf { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}
