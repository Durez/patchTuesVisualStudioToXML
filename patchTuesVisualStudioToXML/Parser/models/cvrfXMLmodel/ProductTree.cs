using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "FullProductName")]
	public class FullProductName
	{

		[XmlAttribute(AttributeName = "ProductID")]
		public int ProductID { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Branch")]
	public class Branch
	{

		[XmlElement(ElementName = "FullProductName")]
		public List<FullProductName> FullProductName { get; set; }

		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }

		[XmlAttribute(AttributeName = "Name")]
		public string Name { get; set; }

		[XmlText]
		public string Text { get; set; }

		[XmlElement(ElementName = "Branch")]
		public List<Branch> Branchs { get; set; }
	}

	[XmlRoot(ElementName = "ProductTree")]
	public class ProductTree
	{

		[XmlElement(ElementName = "Branch")]
		public Branch Branch { get; set; }

		[XmlElement(ElementName = "FullProductName")]
		public List<FullProductName> FullProductNames { get; set; }
	}

}
