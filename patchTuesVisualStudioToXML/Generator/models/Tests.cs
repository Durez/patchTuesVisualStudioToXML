using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Generator.models
{
	[XmlRoot(ElementName = "object")]
	public class @object
	{

		[XmlAttribute(AttributeName = "object_ref")]
		public string object_ref { get; set; }
	}

	[XmlRoot(ElementName = "state")]
	public class state
	{

		[XmlAttribute(AttributeName = "state_ref")]
		public string state_ref { get; set; }
	}

	[XmlRoot(ElementName = "registry_test")]
	public class registry_test
	{

		[XmlElement(ElementName = "object")]
		public @object @object { get; set; }

		[XmlElement(ElementName = "state")]
		public List<state> state { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlAttribute(AttributeName = "check_existence")]
		public string check_existence { get; set; }

		[XmlAttribute(AttributeName = "check")]
		public string check { get; set; }
	}

	[XmlRoot(ElementName = "file_test")]
	public class file_test
	{

		[XmlElement(ElementName = "object")]
		public @object @object { get; set; }

		[XmlElement(ElementName = "state")]
		public List<state> state { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlAttribute(AttributeName = "check_existence")]
		public string check_existence { get; set; }

		[XmlAttribute(AttributeName = "check")]
		public string check { get; set; }
	}

	[XmlRoot(ElementName = "tests")]
	public class tests
	{

		[XmlElement(ElementName = "registry_test")]
		public List<registry_test> registry_test { get; set; }

		[XmlElement(ElementName = "file_test")]
		public List<file_test> file_test { get; set; }
	}


}
