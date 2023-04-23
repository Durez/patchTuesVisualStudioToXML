using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "FullProductName", Namespace = "http://www.icasi.org/CVRF/schema/prod/1.1")]
	public class FullProductName
	{

		[XmlAttribute(AttributeName = "ProductID")]
		public string ProductID { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Branch", Namespace = "http://www.icasi.org/CVRF/schema/prod/1.1")]
	public class Branch
	{

		[XmlElement(ElementName = "FullProductName", Namespace = "http://www.icasi.org/CVRF/schema/prod/1.1")]
		public List<FullProductName> FullProductNamesList { get; set; }

		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }

		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }

		[XmlText]
		public string Text { get; set; }

		[XmlElement(ElementName = "Branch", Namespace = "http://www.icasi.org/CVRF/schema/prod/1.1")]
		public List<Branch> BranchsList { get; set; }
	}


	[XmlRoot(ElementName = "ProductTree", Namespace = "http://www.icasi.org/CVRF/schema/prod/1.1")]
	public class ProductTree
	{

		[XmlElement(ElementName = "Branch", Namespace = "http://www.icasi.org/CVRF/schema/prod/1.1")]
		public Branch Branch { get; set; }

		[XmlElement(ElementName = "FullProductName", Namespace = "http://www.icasi.org/CVRF/schema/prod/1.1")]
		public List<FullProductName> FullProductNamesList { get; set; }
	}

}
