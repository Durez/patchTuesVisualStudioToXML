using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.GeneratorXML.models
{
	[XmlRoot(ElementName = "object")]
	public class ObjectTAG
	{

		[XmlAttribute(AttributeName = "object_ref")]
		public string objectRef { get; set; }
	}

	[XmlRoot(ElementName = "state")]
	public class State
	{

		[XmlAttribute(AttributeName = "state_ref")]
		public string stateRef { get; set; }
	}

	[XmlRoot(ElementName = "registry_test")]
	public class RegistryTest
	{

		[XmlElement(ElementName = "object")]
		public ObjectTAG objectTag { get; set; }

		[XmlElement(ElementName = "state")]
		public List<State> states { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlAttribute(AttributeName = "check_existence")]
		public string checkExistence { get; set; }

		[XmlAttribute(AttributeName = "check")]
		public string check { get; set; }
	}

	[XmlRoot(ElementName = "file_test")]
	public class FileTest
	{

		[XmlElement(ElementName = "object")]
		public ObjectTAG @object { get; set; }

		[XmlElement(ElementName = "state")]
		public List<State> state { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlAttribute(AttributeName = "check_existence")]
		public string checkExistence { get; set; }

		[XmlAttribute(AttributeName = "check")]
		public string check { get; set; }
	}

	[XmlRoot(ElementName = "tests")]
	public class Tests
	{

		[XmlElement(ElementName = "registry_test")]
		public List<RegistryTest> registryTest { get; set; }

		[XmlElement(ElementName = "file_test")]
		public List<FileTest> fileTest { get; set; }
	}


}
