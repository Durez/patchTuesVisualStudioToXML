using patchTuesVisualStudioToXML.GeneratorXML;
using patchTuesVisualStudioToXML.GeneratorXML.models;
using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser
{
    static public class ParserMSRCXMLtoOVALXML
    {
        static private XmlSerializer serializer = new XmlSerializer(typeof(OvalDefinitions));
        static public Cvrfdoc GetProductData(string productname = "Visual Studio") 
        { 
            MSrcAPIController controller =  new MSrcAPIController();
            DateTime timestart = DateTime.Now;
            Console.WriteLine("Start using Microsoft API");
            Cvrfdoc doc =  controller.GETXMLCvrfAsync(controller.GETRawCvrfURLAsync().GetAwaiter().GetResult()).GetAwaiter().GetResult();
            string duration = (DateTime.Now - timestart).TotalSeconds.ToString("0.##");
            Console.WriteLine("Create CVRFXML Document is done. Duration: " + duration + " seconds");
            return doc;
        }

        static public void WriteOVALXMLToFile(OvalDefinitions resultdoc, string nameOfFile = "res")
        {
            Stream fs = new FileStream(nameOfFile + ".xml", FileMode.Create);
            XmlWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
            writer.Settings.Indent = true;
            var ns = new XmlSerializerNamespaces();
            ns.Add("oval", "http://oval.mitre.org/XMLSchema/oval-common-5");
            ns.Add("schemaLocation", "http://oval.mitre.org/XMLSchema/oval-definitions-5 oval-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#windows windows-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#independent independent-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-common-5 oval-common-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#linux linux-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#unix unix-definitions-schema.xsd");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            serializer.Serialize(writer, resultdoc, ns);
            fs.Close();
            Console.WriteLine("Writing OVAL XML to file complited.");
        }

        static public OvalDefinitions LoadSample()
        {
            using (StreamReader reader = new StreamReader("sample.xml"))
            {
                var test = (OvalDefinitions)serializer.Deserialize(reader);
                return test;
            }
        }
    }
}
