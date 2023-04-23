using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.GeneratorXML.models
{
	[XmlRoot(ElementName = "value")]
	public class value
	{
		//TODO datatype and operation
		[XmlAttribute(AttributeName = "operation")]
		public string operation { get; set; }

		[XmlAttribute(AttributeName = "datatype")]
		public string datatype { get; set; }

		[XmlText]
		public string text { get; set; }
	}

	[XmlRoot(ElementName = "registry_state")]
	public class registry_state
	{

		[XmlElement(ElementName = "value")]
		public value value { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlText]
		public string text { get; set; }
	}

	[XmlRoot(ElementName = "version")]
	public class version
	{

		[XmlAttribute(AttributeName = "datatype")]
		public string datatype { get; set; }

		[XmlAttribute(AttributeName = "operation")]
		public string operation { get; set; }

		[XmlText]
		public string text { get; set; }
	}

	[XmlRoot(ElementName = "file_state")]
	public class file_state
	{

		[XmlElement(ElementName = "version")]
		public List<version> version { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlText]
		public string text { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string versionATR { get; set; }
	}

	[XmlRoot(ElementName = "states")]
	public class states
	{

		[XmlElement(ElementName = "registry_state")]
		public List<registry_state> registry_state { get; set; }

		[XmlElement(ElementName = "file_state")]
		public List<file_state> file_state { get; set; }
	}

}
