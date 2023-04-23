using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Generator.models
{
	[XmlRoot(ElementName = "definitions")]
	public class definitions
	{
		[XmlElement(ElementName = "definition")]
		public List<definition> defs { get; set; }
	}

	[XmlRoot(ElementName = "affected")]
	public class Affected
	{

		[XmlElement(ElementName = "platform")]
		public List<string> platforms { get; set; }

		[XmlElement(ElementName = "product")]
		public List<string> products { get; set; }

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
	public class Metadata
	{

		[XmlElement(ElementName = "title")]
		public string title { get; set; }

		[XmlElement(ElementName = "affected")]
		public Affected affected { get; set; }

		[XmlElement(ElementName = "reference")]
		public List<Reference> reference { get; set; }

		[XmlElement(ElementName = "description")]
		public string description { get; set; }
	}

	[XmlRoot(ElementName = "extend_definition")]
	public class ExtendDefinition
	{

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlAttribute(AttributeName = "definition_ref")]
		public string DefinitionRef { get; set; }
	}

	[XmlRoot(ElementName = "criterion")]
	public class Criterion
	{

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlAttribute(AttributeName = "test_ref")]
		public string TestRef { get; set; }
	}

	[XmlRoot(ElementName = "criteria")]
	public class Criteria
	{

		[XmlElement(ElementName = "criterion")]
		public List<Criterion> Criterions { get; set; }

		[XmlAttribute(AttributeName = "operator")]
		public string Operator { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlElement(ElementName = "extend_definition")]
		public ExtendDefinition ExtendDefinition { get; set; }

		[XmlElement(ElementName = "criteria")]
		public List<Criteria> Criters { get; set; }
	}


	[XmlRoot(ElementName = "definition")]
	public class definition
	{

		[XmlElement(ElementName = "metadata")]
		public Metadata metadata { get; set; }

		[XmlElement(ElementName = "criteria")]
		public Criteria criteria { get; set; }

		[XmlAttribute(AttributeName = "class")]
		public string @class { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public int version { get; set; }

		[XmlText]
		public string text { get; set; }
	}
}
