using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Generator.models
{
	[XmlRoot(ElementName = "object_component")]
	public class object_component
	{

		[XmlAttribute(AttributeName = "item_field")]
		public string item_field { get; set; }

		[XmlAttribute(AttributeName = "object_ref")]
		public string object_ref { get; set; }
	}

	[XmlRoot(ElementName = "concat")]
	public class concat
	{

		[XmlElement(ElementName = "object_component")]
		public object_component object_component { get; set; }

		[XmlElement(ElementName = "literal_component")]
		public string literal_component { get; set; }
	}

	[XmlRoot(ElementName = "local_variable")]
	public class local_variable
	{

		[XmlElement(ElementName = "concat")]
		public concat concat { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlAttribute(AttributeName = "datatype")]
		public string datatype { get; set; }

		[XmlText]
		public string text { get; set; }

		[XmlElement(ElementName = "object_component")]
		public object_component object_component { get; set; }
	}

	[XmlRoot(ElementName = "variables")]
	public class variables
	{

		[XmlElement(ElementName = "local_variable")]
		public List<local_variable> local_variable { get; set; }
	}


}
