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
		public string ObjectRef { get; set; }
        public ObjectTAG()
        {

        }
        public ObjectTAG(string objectRef)
        {
            this.ObjectRef = objectRef;
        }
    }

	[XmlRoot(ElementName = "state")]
	public class State
	{

		[XmlAttribute(AttributeName = "state_ref")]
		public string StateRef { get; set; }
        public State()
        {

        }
		public State(string stateRef)
        {
            this.StateRef = stateRef;
        }
    }

	[XmlRoot(ElementName = "registry_test")]
	public class RegistryTest
	{

		[XmlElement(ElementName = "object")]
		public ObjectTAG ObjectTag { get; set; }

		[XmlElement(ElementName = "state")]
		public List<State> States { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlAttribute(AttributeName = "check_existence")]
		public string CheckExistence { get; set; }

		[XmlAttribute(AttributeName = "check")]
		public string Check { get; set; }
        
		public RegistryTest()
        {

        }
		public RegistryTest(string id, string comment, List<State> states, ObjectTAG objectRef, string checkExistence = "at_least_one_exists", string check = "all", string xmlns = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows", string version = "1")
        {
			this.Id = id;
			this.Xmlns = xmlns;
			this.Comment = comment;
			this.Version = version;
			this.CheckExistence = checkExistence;
			this.Check = check;

			this.States = states;
			this.ObjectTag = objectRef;
		}
	}

	[XmlRoot(ElementName = "file_test")]
	public class FileTest
	{

		[XmlElement(ElementName = "object")]
		public ObjectTAG Object { get; set; }

		[XmlElement(ElementName = "state")]
		public List<State> State { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlAttribute(AttributeName = "check_existence")]

		public string CheckExistence { get; set; }

		[XmlAttribute(AttributeName = "check")]
		public string Check { get; set; }

	}

	[XmlRoot(ElementName = "tests")]
	public class Tests
	{

		[XmlElement(ElementName = "registry_test", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
		public List<RegistryTest> RegistryTest { get; set; }

		[XmlElement(ElementName = "file_test", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
		public List<FileTest> FileTest { get; set; }
	}


}
