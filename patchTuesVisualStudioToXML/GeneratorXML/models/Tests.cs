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
        public ObjectTAG()
        {

        }
        public ObjectTAG(string objectRef)
        {
            this.objectRef = objectRef;
        }
    }

	[XmlRoot(ElementName = "state")]
	public class State
	{

		[XmlAttribute(AttributeName = "state_ref")]
		public string stateRef { get; set; }
        public State()
        {

        }
		public State(string stateRef)
        {
            this.stateRef = stateRef;
        }
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
        
		public RegistryTest()
        {

        }
		public RegistryTest(string id, string comment, List<State> states, ObjectTAG objectRef, string checkExistence = "at_least_one_exists", string check = "all", string xmlns = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows", string version = "1")
        {
			this.id = id;
			this.xmlns = xmlns;
			this.comment = comment;
			this.version = version;
			this.checkExistence = checkExistence;
			this.check = check;

			this.states = states;
			this.objectTag = objectRef;
		}
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

		[XmlElement(ElementName = "registry_test", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
		public List<RegistryTest> registryTest { get; set; }

		[XmlElement(ElementName = "file_test", Namespace = "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows")]
		public List<FileTest> fileTest { get; set; }
	}


}
