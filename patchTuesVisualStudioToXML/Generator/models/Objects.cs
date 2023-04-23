using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Generator.models
{
	[XmlRoot(ElementName = "path")]
	public class path
	{

		[XmlAttribute(AttributeName = "var_ref")]
		public string var_ref { get; set; }

		[XmlAttribute(AttributeName = "var_check")]
		public string var_check { get; set; }
	}

	[XmlRoot(ElementName = "file_object")]
	public class file_object
	{

		[XmlElement(ElementName = "path")]
		public path path { get; set; }

		[XmlElement(ElementName = "filename")]
		public string filename { get; set; }

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

	[XmlRoot(ElementName = "filter")]
	public class filter
	{

		[XmlAttribute(AttributeName = "action")]
		public string action { get; set; }

		[XmlText]
		public string text { get; set; }
	}

	[XmlRoot(ElementName = "set")]
	public class set
	{

		[XmlElement(ElementName = "object_reference")]
		public string object_reference { get; set; }

		[XmlElement(ElementName = "filter")]
		public filter filter { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlText]
		public string text { get; set; }
	}

	[XmlRoot(ElementName = "registry_object")]
	public class registry_object
	{

		[XmlElement(ElementName = "set")]
		public set set { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlAttribute(AttributeName = "ns1")]
		public string ns1 { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlText]
		public string text { get; set; }

		[XmlElement(ElementName = "behaviors")]
		public behaviors behaviors { get; set; }

		[XmlElement(ElementName = "hive")]
		public string hive { get; set; }

		[XmlElement(ElementName = "key")]
		public key key { get; set; }

		[XmlElement(ElementName = "name")]
		public string name { get; set; }
	}

	[XmlRoot(ElementName = "behaviors")]
	public class behaviors
	{

		[XmlAttribute(AttributeName = "windows_view")]
		public string windows_view { get; set; }
	}

	[XmlRoot(ElementName = "key")]
	public class key
	{

		[XmlAttribute(AttributeName = "var_ref")]
		public string var_ref { get; set; }

		[XmlAttribute(AttributeName = "var_check")]
		public string var_check { get; set; }
	}

	[XmlRoot(ElementName = "objects")]
	public class objects
	{

		[XmlElement(ElementName = "file_object")]
		public List<file_object> file_object { get; set; }

		[XmlElement(ElementName = "registry_object")]
		public List<registry_object> registry_object { get; set; }
	}


}
