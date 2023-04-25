using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel
{
	[XmlRoot(ElementName = "DocumentNotes")]
	public class DocumentNotes
	{

		[XmlElement(ElementName = "Note")]
		public List<Note> NotesList { get; set; }
	}
}
