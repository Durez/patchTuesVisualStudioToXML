using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.GeneratorXML.models
{
	[XmlRoot(ElementName = "generator")]
	public class GeneratorTAG
	{

		[XmlElement(ElementName = "product_name", Namespace = "http://oval.mitre.org/XMLSchema/oval-common-5")]
		public string product_name { get; set; }

		[XmlElement(ElementName = "schema_version", Namespace = "http://oval.mitre.org/XMLSchema/oval-common-5")]
		public string schema_version { get; set; }

		[XmlElement(ElementName = "timestamp", Namespace = "http://oval.mitre.org/XMLSchema/oval-common-5")]
		public string timestamp { get; set; }
	}
}
