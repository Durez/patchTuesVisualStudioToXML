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
		public string operation { get; set; }

		[XmlAttribute(AttributeName = "datatype")]
		public string datatype { get; set; }

		[XmlText]
		public string text { get; set; }
        public ValueTAG()
        {

        }
        public ValueTAG(string operation, string datatype, string text)
        {
            this.operation = operation;
            this.datatype = datatype;
            this.text = text;
        }
    }

	[XmlRoot(ElementName = "registry_state", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
	public class RegistryState
	{

		[XmlElement(ElementName = "value")]
		public ValueTAG value { get; set; }

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
        public RegistryState()
        {

        }
        public RegistryState(ValueTAG value, string id, string version, string comment, string? text = null)
        {
            this.value = value;
            this.id = id;
            this.version = version;
            this.comment = comment;
            if (text != null) this.text = text;
        }

        public RegistryState(string id, string comment)
        {
			this.id = id;
			this.xmlns = xmlns;
			this.version = "1";
			this.comment = comment;
        }
    }

	[XmlRoot(ElementName = "version")]
	public class VersionTAG
	{

		[XmlAttribute(AttributeName = "datatype")]
		public string datatype { get; set; }

		[XmlAttribute(AttributeName = "operation")]
		public string operation { get; set; }

		[XmlText]
		public string text { get; set; }
	}

	[XmlRoot(ElementName = "file_state")]
	public class FileStateTAG
	{

		[XmlElement(ElementName = "version")]
		public List<VersionTAG> version { get; set; }

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
	public class States
	{

		[XmlElement(ElementName = "registry_state", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
		public List<RegistryState> registryState { get; set; }

		[XmlElement(ElementName = "file_state", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
		public List<FileStateTAG> fileState { get; set; }
        public States()
        {

        }

        public States(List<RegistryState> registryState, List<FileStateTAG> fileState)
        {
            this.registryState = registryState;
            this.fileState = fileState;
        }
    }

}
