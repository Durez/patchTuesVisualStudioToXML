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
		public string? VarRef { get; set; }

		[XmlAttribute(AttributeName = "var_check")]
		public string? VarCheck { get; set; }
	}

	[XmlRoot(ElementName = "file_object")]
	public class FileObject
	{

		[XmlElement(ElementName = "path")]
		public PathTAG? Path { get; set; }

		[XmlElement(ElementName = "filename")]
		public string? Filename { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "filter")]
	public class Filter
	{

		[XmlAttribute(AttributeName = "action")]
		public string Action { get; set; }

		[XmlText]
        public string Text { get; set; }
	}

	[XmlRoot(ElementName = "set")]
	public class SetTAG
	{

		[XmlElement(ElementName = "object_reference")]
		public string ObjectReference { get; set; }

		[XmlElement(ElementName = "filter")]
		public Filter Filter { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "registry_object")]
	public class RegistryObject
	{

		[XmlElement(ElementName = "set", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5")]
		public SetTAG Set { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlAttribute(AttributeName = "ns1")]
		public string Ns1 { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlText]
		public string Text { get; set; }

		[XmlElement(ElementName = "behaviors")]
		public Behaviors Behaviors { get; set; }

		[XmlElement(ElementName = "hive")]
		public string Hive { get; set; }

		[XmlElement(ElementName = "key")]
		public KeyTAG Key { get; set; }

		[XmlElement(ElementName = "name")]
		public string Name { get; set; }

        public RegistryObject()
        {

        }

        public RegistryObject(string id, string comment, Behaviors? behaviors, string? hive, KeyTAG key, string name, string xmlns = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows", string version = "1", SetTAG? set = null, string? text = null)
        {
            this.Xmlns = xmlns;
            this.Id = id;
            this.Version = version;
            this.Comment = comment;
			if (text != null) this.Text = text;
			if (hive != null) this.Hive = hive;
            this.Name = name;

            this.Key = key;
			if (behaviors != null) this.Behaviors = behaviors;
            if (set != null) this.Set = set;
        }
    }

	[XmlRoot(ElementName = "behaviors")]
	public class Behaviors
	{

		[XmlAttribute(AttributeName = "windows_view")]
		public string WindowsView { get; set; }
        public Behaviors()
        {

        }
        public Behaviors(string windowsView)
        {
            this.WindowsView = windowsView;
        }
    }

	[XmlRoot(ElementName = "key")]
	public class KeyTAG
	{

		[XmlAttribute(AttributeName = "var_ref")]
		public string VarRef { get; set; }

		[XmlAttribute(AttributeName = "var_check")]
		public string VarCheck { get; set; }

		[XmlAttribute(AttributeName = "operation")]
		public string Operation { get; set; }

		[XmlText]
		public string Text { get; set; }

        public KeyTAG()
        {

        }

		public KeyTAG(string varRef, string varCheck)
        {
            this.VarRef = varRef;
            this.VarCheck = varCheck;
        }
    }

	[XmlRoot(ElementName = "objects")]
	public class Objects
	{

		[XmlElement(ElementName = "file_object")]
		public List<FileObject> FileObject { get; set; }

		[XmlElement(ElementName = "registry_object", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
		public List<RegistryObject> RegistryObject { get; set; }
	}


}
