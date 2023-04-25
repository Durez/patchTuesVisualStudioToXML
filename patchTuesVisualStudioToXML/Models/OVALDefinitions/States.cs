using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.GeneratorXML.models
{
	[XmlRoot(ElementName = "value")]
	public class ValueTAG
	{
		//TODO datatype and operation
		[XmlAttribute(AttributeName = "operation")]
		public string Operation { get; set; }

		[XmlAttribute(AttributeName = "datatype")]
		public string Datatype { get; set; }

		[XmlText]
		public string Text { get; set; }
        public ValueTAG()
        {

        }
        public ValueTAG(string operation, string datatype, string text)
        {
            this.Operation = operation;
            this.Datatype = datatype;
            this.Text = text;
        }
    }

	[XmlRoot(ElementName = "registry_state", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
	public class RegistryState
	{

		[XmlElement(ElementName = "value")]
		public ValueTAG Value { get; set; }

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
        public RegistryState()
        {

        }
        public RegistryState(ValueTAG value, string id, string version, string comment, string? text = null)
        {
            this.Value = value;
            this.Id = id;
            this.Version = version;
            this.Comment = comment;
            if (text != null) this.Text = text;
        }

        public RegistryState(string id, string comment)
        {
			this.Id = id;
			this.Xmlns = Xmlns;
			this.Version = "1";
			this.Comment = comment;
        }
    }

	[XmlRoot(ElementName = "version")]
	public class VersionTAG
	{

		[XmlAttribute(AttributeName = "datatype")]
		public string? Datatype { get; set; }

		[XmlAttribute(AttributeName = "operation")]
		public string Operation { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "file_state")]
	public class FileStateTAG
	{

		[XmlElement(ElementName = "version")]
		public List<VersionTAG> Version { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlText]
		public string Text { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string VersionATR { get; set; }
	}

	[XmlRoot(ElementName = "states")]
	public class States
	{

		[XmlElement(ElementName = "registry_state", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
		public List<RegistryState> RegistryState { get; set; }

		[XmlElement(ElementName = "file_state", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
		public List<FileStateTAG> FileState { get; set; }
        public States()
        {

        }

        public States(List<RegistryState> registryState, List<FileStateTAG> fileState)
        {
            this.RegistryState = registryState;
            this.FileState = fileState;
        }
    }

}
