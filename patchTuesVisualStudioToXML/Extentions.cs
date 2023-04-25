using patchTuesVisualStudioToXML.GeneratorXML.models;
using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML
{
    public class Extentions
    {
        private XmlSerializer serializer = new XmlSerializer(typeof(OvalDefinitions));
        Cvrfdoc LoadSampleCVRF()
        {
            using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "/Resourses/2023-Apr.xml"))
            {
                var test = (Cvrfdoc)serializer.Deserialize(reader);
                return test;
            }
        }
        public void WriteOVALXMLToFile(OvalDefinitions resultdoc, string fullNameOfFile = "res.xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OvalDefinitions));
            using (Stream fs = new FileStream("Resourses/" + fullNameOfFile, FileMode.Create))
            {
                XmlWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
                var ns = new XmlSerializerNamespaces();
                ns.Add("oval", "http://oval.mitre.org/XMLSchema/oval-common-5");
                ns.Add("schemaLocation", "http://oval.mitre.org/XMLSchema/oval-definitions-5 oval-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#windows windows-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#independent independent-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-common-5 oval-common-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#linux linux-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#unix unix-definitions-schema.xsd");
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                serializer.Serialize(writer, resultdoc, ns);
            }
            Console.WriteLine("Writing OVAL XML to file complited.");
        }
    }
}
