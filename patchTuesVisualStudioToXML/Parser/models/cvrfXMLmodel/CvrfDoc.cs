using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "cvrfdoc", Namespace = "http://www.icasi.org/CVRF/schema/cvrf/1.1")]
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
		public List<Vulnerability> VulnerabilitiesList { get; set; }

		[XmlAttribute(AttributeName = "cpe-lang", Namespace = "http://cpe.mitre.org/language/2.0")]
		public string CpeLang { get; set; }

		[XmlAttribute(AttributeName = "scap-core", Namespace = "https://scap.nist.gov/schema/scap-core/1.0")]
		public string ScapCore { get; set; }

		[XmlAttribute(AttributeName = "cvrf-common", Namespace = "http://www.icasi.org/CVRF/schema/common/1.1")]
		public string CvrfCommon { get; set; }

		[XmlAttribute(AttributeName = "dc", Namespace = "http://purl.org/dc/elements/1.1/")]
		public string Dc { get; set; }

		[XmlAttribute(AttributeName = "prod", Namespace = "http://www.icasi.org/CVRF/schema/prod/1.1")]
		public string Prod { get; set; }

		[XmlAttribute(AttributeName = "cvssv2", Namespace = "https://scap.nist.gov/schema/cvss-v2/1.0")]
		public string Cvssv2 { get; set; }

		[XmlAttribute(AttributeName = "vuln", Namespace = "http://www.icasi.org/CVRF/schema/vuln/1.1")]
		public string Vuln { get; set; }

		[XmlAttribute(AttributeName = "sch", Namespace = "http://purl.oclc.org/dsdl/schematron")]
		public string Sch { get; set; }

		[XmlAttribute(AttributeName = "cvrf", Namespace = "http://www.icasi.org/CVRF/schema/cvrf/1.1")]
		public string Cvrf { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}
