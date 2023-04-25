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
		public List<Definition> DefinitionsList { get; set; }
        public Definitions()
        {

        }
	}

	[XmlRoot(ElementName = "affected")]
	public class Affected
	{

		[XmlElement(ElementName = "platform")]
		public List<string>? PlatformsList { get; set; }

		[XmlElement(ElementName = "product")]
		public List<string>? ProductsList { get; set; }

		[XmlAttribute(AttributeName = "family")]
		public string? Family { get; set; }

		[XmlText]
		public string? Text { get; set; }

	}

	[XmlRoot(ElementName = "reference")]
	public class Reference
	{

		[XmlAttribute(AttributeName = "source")]
		public string? Source { get; set; }

		[XmlAttribute(AttributeName = "ref_id")]
		public string? RefId { get; set; }

		[XmlAttribute(AttributeName = "ref_url")]
		public string? RefUrl { get; set; }
	}

	[XmlRoot(ElementName = "metadata")]
	public class MetadataTAG
	{

		[XmlElement(ElementName = "title")]
		public string Title { get; set; }

		[XmlElement(ElementName = "affected")]
		public Affected Affected { get; set; }

		[XmlElement(ElementName = "reference")]
		public List<Reference> ReferencesList { get; set; }

		[XmlElement(ElementName = "description")]
		public string Description { get; set; }
        public MetadataTAG()
        {

        }

	}

	[XmlRoot(ElementName = "extend_definition")]
	public class ExtendDefinition
	{

		[XmlAttribute(AttributeName = "comment")]
		public string? Comment { get; set; }

		[XmlAttribute(AttributeName = "definition_ref")]
		public string? DefinitionRef { get; set; }
	}

	[XmlRoot(ElementName = "criterion")]
	public class Criterion
	{

		[XmlAttribute(AttributeName = "comment")]
		public string Comment { get; set; }

		[XmlAttribute(AttributeName = "test_ref")]
		public string TestRef { get; set; }
        public Criterion()
        {
				
        }

        public Criterion(string id, string comment)
        {
            this.Comment = comment;
            this.TestRef = id;
        }
    }

	[XmlRoot(ElementName = "criteria")]
	public class Criteria
	{

		[XmlAttribute(AttributeName = "operator")]
		public string? Operator { get; set; }

		[XmlAttribute(AttributeName = "comment")]
		public string? Comment { get; set; }

		[XmlElement(ElementName = "extend_definition")]
		public ExtendDefinition? ExtendDefinition { get; set; }
		[XmlElement(ElementName = "criterion")]

		public List<Criterion>? CriterionsList { get; set; }

		[XmlElement(ElementName = "criteria")]
		public List<Criteria>? CritersList { get; set; }
	}


	[XmlRoot(ElementName = "definition")]
	public class Definition
	{

		[XmlElement(ElementName = "metadata")]
		public MetadataTAG? Metadata { get; set; }

		[XmlElement(ElementName = "criteria")]
		public Criteria? Criteria { get; set; }

		[XmlAttribute(AttributeName = "class")]
		public string? Class { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[XmlText]
		public string Text { get; set; }
	}
}
