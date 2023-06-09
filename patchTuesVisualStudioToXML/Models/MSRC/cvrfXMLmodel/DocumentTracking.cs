﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "Identification")]
	public class Identification
	{

		[XmlElement(ElementName = "ID")]
		public string ID { get; set; }

		[XmlElement(ElementName = "Alias")]
		public string Alias { get; set; }
	}


	[XmlRoot(ElementName = "DocumentTracking")]
	public class DocumentTracking
	{

		[XmlElement(ElementName = "Identification")]
		public Identification Identification { get; set; }

		[XmlElement(ElementName = "Status")]
		public string Status { get; set; }

		[XmlElement(ElementName = "Version")]
		public double Version { get; set; }

		[XmlElement(ElementName = "RevisionHistory")]
		public RevisionHistory RevisionHistory { get; set; }

		[XmlElement(ElementName = "InitialReleaseDate")]
		public string InitialReleaseDate { get; set; }

		[XmlElement(ElementName = "CurrentReleaseDate")]
		public string CurrentReleaseDate { get; set; }
	}
}
