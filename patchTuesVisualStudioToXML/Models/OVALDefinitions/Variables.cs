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
		public string ItemField { get; set; }

		[XmlAttribute(AttributeName = "object_ref")]
		public string ObjectRef { get; set; }
        public ObjectComponent()
        {

        }
        public ObjectComponent(string itemField, string objectRef)
        {
            this.ItemField = itemField;
            this.ObjectRef = objectRef;
        }
    }

	[XmlRoot(ElementName = "concat")]
	public class Concat
	{

		[XmlElement(ElementName = "object_component")]
		public ObjectComponent ObjectComponent { get; set; }

		[XmlElement(ElementName = "literal_component")]
		public string LiteralComponent { get; set; }
	}

	[XmlRoot(ElementName = "local_variable")]
	public class LocalVariable
	{

		[XmlElement(ElementName = "concat")]
		public Concat Concat { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlAttribute(AttributeName = "datatype")]
		public string Datatype { get; set; }

		[XmlText]
		public string Text { get; set; }

		[XmlElement(ElementName = "object_component")]
		public ObjectComponent ObjectComponent { get; set; }

        public LocalVariable()
        {

        }
        public LocalVariable( string id, string version, string comment, string datatype, ObjectComponent objectComponent, Concat? concat = null, string? text = null)
        {
            if (concat != null) this.Concat = concat;
            this.Id = id;
            this.Version = version;
            this.Comment = comment;
            this.Datatype = datatype;
			if (text != null)  this.Text = text;
            this.ObjectComponent = objectComponent;
        }
    }

	[XmlRoot(ElementName = "variables")]
	public class Variables
	{

		[XmlElement(ElementName = "local_variable")]
		public List<LocalVariable> LocalVariable { get; set; }
	}


}
