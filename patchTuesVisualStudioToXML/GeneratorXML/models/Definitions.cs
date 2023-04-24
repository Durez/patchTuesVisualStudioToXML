using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.GeneratorXML.models
{
	[XmlRoot(ElementName = "definitions")]
	public class Definitions
	{
		[XmlElement(ElementName = "definition")]
		public List<Definition> definitionsList { get; set; }
        public Definitions()
        {
			definitionsList = new List<Definition>();

		}
	}

	[XmlRoot(ElementName = "affected")]
	public class Affected
	{

		[XmlElement(ElementName = "platform")]
		public List<string> platformsList { get; set; }

		[XmlElement(ElementName = "product")]
		public List<string> productsList { get; set; }

		[XmlAttribute(AttributeName = "family")]
		public string family { get; set; }

		[XmlText]
		public string text { get; set; }

	}

	[XmlRoot(ElementName = "reference")]
	public class Reference
	{

		[XmlAttribute(AttributeName = "source")]
		public string source { get; set; }

		[XmlAttribute(AttributeName = "ref_id")]
		public string ref_id { get; set; }

		[XmlAttribute(AttributeName = "ref_url")]
		public string ref_url { get; set; }
	}

	[XmlRoot(ElementName = "metadata")]
	public class MetadataTAG
	{

		[XmlElement(ElementName = "title")]
		public string title { get; set; }

		[XmlElement(ElementName = "affected")]
		public Affected affected { get; set; }

		[XmlElement(ElementName = "reference")]
		public List<Reference> referencesList { get; set; }

		[XmlElement(ElementName = "description")]
		public string description { get; set; }
        public MetadataTAG()
        {
			referencesList = new List<Reference>();
		}
	}

	[XmlRoot(ElementName = "extend_definition")]
	public class ExtendDefinition
	{

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlAttribute(AttributeName = "definition_ref")]
		public string definitionRef { get; set; }
	}

	[XmlRoot(ElementName = "criterion")]
	public class Criterion
	{

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlAttribute(AttributeName = "test_ref")]
		public string testRef { get; set; }
	}

	[XmlRoot(ElementName = "criteria")]
	public class Criteria
	{

		[XmlAttribute(AttributeName = "operator")]
		public string @operator { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string comment { get; set; }

		[XmlElement(ElementName = "extend_definition")]
		public ExtendDefinition extendDefinition { get; set; }
		[XmlElement(ElementName = "criterion")]

		public List<Criterion> criterionsList { get; set; }

		[XmlElement(ElementName = "criteria")]
		public List<Criteria> critersList { get; set; }
	}


	[XmlRoot(ElementName = "definition")]
	public class Definition
	{

		[XmlElement(ElementName = "metadata")]
		public MetadataTAG metadata { get; set; }

		[XmlElement(ElementName = "criteria")]
		public Criteria criteria { get; set; }

		[XmlAttribute(AttributeName = "class")]
		public string @class { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string version { get; set; }

		[XmlText]
		public string text { get; set; }
	}
}
