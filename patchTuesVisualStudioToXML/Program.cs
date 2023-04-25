using patchTuesVisualStudioToXML.GeneratorXML;
using patchTuesVisualStudioToXML.Models.OVALDefinitions;
using patchTuesVisualStudioToXML.Parser;
using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using patchTuesVisualStudioToXML.Validator;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

Console.WriteLine("Attempting to validate");
//Console.WriteLine("sample {0}", Validator.XMLISValidated() ? "did not validate" : "validated");

XmlSerializer serializer = new XmlSerializer(typeof(OvalDefinitions));
void WriteOVALXMLToFile(OvalDefinitions resultdoc, string fullNameOfFile = "res.xml")
{
    using (Stream fs = new FileStream("Resourses/"+fullNameOfFile, FileMode.Create))
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

OvalDefinitions LoadSample()
{
    using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "/Resourses/sample.xml"))
    {
        var test = (OvalDefinitions)serializer.Deserialize(reader);
        return test;
    }
}

Cvrfdoc LoadSampleCVRF()
{
    using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "/Resourses/2023-Apr.xml"))
    {
        var test = (Cvrfdoc)serializer.Deserialize(reader);
        return test;
    }
}

MSrcAPIController controller = new MSrcAPIController();
DateTime timestart = DateTime.Now;
Console.WriteLine("Start using Microsoft API");
Cvrfdoc rawDoc = controller.GETXMLCvrfAsync(controller.GETRawCvrfURLAsync().GetAwaiter().GetResult()).GetAwaiter().GetResult();
//Cvrfdoc rawDoc = LoadSampleCVRF();
string duration = (DateTime.Now - timestart).TotalSeconds.ToString("0.##");
Console.WriteLine("Create CVRFXML Document is done. Duration: " + duration + " seconds");

OvalDefinitions result = new GeneratorXMLData(LoadSample()).GenerateXMLData(rawDoc);

WriteOVALXMLToFile(result);
Console.WriteLine(new Validator().ISOVALXMLValidated(fullNameOfXMLfile: "res.xml"));

Console.ReadLine();