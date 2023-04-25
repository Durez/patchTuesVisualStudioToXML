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
		public string productName { get; set; }

		[XmlElement(ElementName = "schema_version", Namespace = "http://oval.mitre.org/XMLSchema/oval-common-5")]
		public string schemaVersion { get; set; }

		[XmlElement(ElementName = "timestamp", Namespace = "http://oval.mitre.org/XMLSchema/oval-common-5")]
		public string timestamp { get; set; }

        public GeneratorTAG()
        {

        }
        public GeneratorTAG(string? schemaVersion = null, string? productName = null)
        {
            if (productName != null) this.productName = productName;
			if (schemaVersion != null) this.schemaVersion = schemaVersion;
            this.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
		}
    }
}
