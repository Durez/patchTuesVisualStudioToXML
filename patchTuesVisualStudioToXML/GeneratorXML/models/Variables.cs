using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.GeneratorXML.models
{
	[XmlRoot(ElementName = "object_component")]
	public class ObjectComponent
	{

		[XmlAttribute(AttributeName = "item_field")]
		public string itemField { get; set; }

		[XmlAttribute(AttributeName = "object_ref")]
		public string objectRef { get; set; }

        public ObjectComponent(string itemField, string objectRef)
        {
            this.itemField = itemField;
            this.objectRef = objectRef;
        }
    }

	[XmlRoot(ElementName = "concat")]
	public class Concat
	{

		[XmlElement(ElementName = "object_component")]
		public ObjectComponent objectComponent { get; set; }

		[XmlElement(ElementName = "literal_component")]
		public string literalComponent { get; set; }
	}

	[XmlRoot(ElementName = "local_variable")]
	public class LocalVariable
	{

		[XmlElement(ElementName = "concat")]
		public Concat concat { get; set; }

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
		public ObjectComponent objectComponent { get; set; }

        public LocalVariable( string id, string version, string comment, string datatype, ObjectComponent objectComponent, Concat? concat = null, string? text = null)
        {
            if (concat != null) this.concat = concat;
            this.id = id;
            this.version = version;
            this.comment = comment;
            this.datatype = datatype;
			if (text != null)  this.text = text;
            this.objectComponent = objectComponent;
        }
    }

	[XmlRoot(ElementName = "variables")]
	public class Variables
	{

		[XmlElement(ElementName = "local_variable")]
		public List<LocalVariable> localVariable { get; set; }
	}


}
