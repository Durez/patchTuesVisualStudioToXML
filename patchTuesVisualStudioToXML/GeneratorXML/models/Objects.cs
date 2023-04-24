using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.GeneratorXML.models
{
	[XmlRoot(ElementName = "path")]
	public class PathTAG
	{

		[XmlAttribute(AttributeName = "var_ref")]
		public string varRef { get; set; }

		[XmlAttribute(AttributeName = "var_check")]
		public string varCheck { get; set; }
	}

	[XmlRoot(ElementName = "file_object")]
	public class FileObject
	{

		[XmlElement(ElementName = "path")]
		public PathTAG path { get; set; }

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
	public class Filter
	{

		[XmlAttribute(AttributeName = "action")]
		public string action { get; set; }

		[XmlText]
		public string text { get; set; }
	}

	[XmlRoot(ElementName = "set")]
	public class SetTAG
	{

		[XmlElement(ElementName = "object_reference")]
		public string objectReference { get; set; }

		[XmlElement(ElementName = "filter")]
		public Filter filter { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlText]
		public string text { get; set; }
	}

	[XmlRoot(ElementName = "registry_object")]
	public class RegistryObject
	{

		[XmlElement(ElementName = "set")]
		public SetTAG set { get; set; }

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
		public Behaviors behaviors { get; set; }

		[XmlElement(ElementName = "hive")]
		public string hive { get; set; }

		[XmlElement(ElementName = "key")]
		public KeyTAG key { get; set; }

		[XmlElement(ElementName = "name")]
		public string name { get; set; }
	}

	[XmlRoot(ElementName = "behaviors")]
	public class Behaviors
	{

		[XmlAttribute(AttributeName = "windows_view")]
		public string windowsView { get; set; }
	}

	[XmlRoot(ElementName = "key")]
	public class KeyTAG
	{

		[XmlAttribute(AttributeName = "var_ref")]
		public string varRef { get; set; }

		[XmlAttribute(AttributeName = "var_check")]
		public string varCheck { get; set; }
	}

	[XmlRoot(ElementName = "objects")]
	public class Objects
	{

		[XmlElement(ElementName = "file_object")]
		public List<FileObject> fileObject { get; set; }

		[XmlElement(ElementName = "registry_object")]
		public List<RegistryObject> registryObject { get; set; }
	}


}
