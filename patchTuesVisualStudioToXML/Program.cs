using patchTuesVisualStudioToXML;
using patchTuesVisualStudioToXML.DAL;
using patchTuesVisualStudioToXML.DAL.Entities;
using patchTuesVisualStudioToXML.GeneratorXML;
using patchTuesVisualStudioToXML.GeneratorXML.models;
using patchTuesVisualStudioToXML.Parser;
using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using patchTuesVisualStudioToXML.Validator;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


string ToStrOVALXML(OvalDefinitions obj)
{
    using (var stringwriter = new System.IO.StringWriter())
    {
        var serializer = new XmlSerializer(typeof(OvalDefinitions));
        serializer.Serialize(stringwriter, obj);
        return stringwriter.ToString();
    }
}

static OvalDefinitions LoadFromOVALXMLString(string xmlText)
{
    using (var stringReader = new System.IO.StringReader(xmlText))
    {
        var serializer = new XmlSerializer(typeof(OvalDefinitions));
        return serializer.Deserialize(stringReader) as OvalDefinitions;
    }
}





MSrcAPIController controller = new MSrcAPIController();
DateTime timestart = DateTime.Now;
Console.WriteLine("Start using Microsoft API");
Cvrfdoc rawDoc = controller.GETXMLCvrfAsync(controller.GETRawCvrfURLAsync().GetAwaiter().GetResult()).GetAwaiter().GetResult();
string duration = (DateTime.Now - timestart).TotalSeconds.ToString("0.##");
Console.WriteLine("Create CVRFXML Document is done. Duration: " + duration + " seconds");





OvalDefinitions result = new GeneratorXMLData(true).GenerateXMLData(rawDoc);
new Extentions().WriteOVALXMLToFile(result);

string res = ToStrOVALXML(result);

using (AppDBContext db = new AppDBContext())
{
    
    OVALXML oVALXML = new OVALXML() { Data = Encoding.Default.GetBytes(res), CreationDate = DateTime.Now };
    db.OVALXMLs.Add(oVALXML);
    db.SaveChanges();

    var xmls = db.OVALXMLs.ToList();
    foreach (var item in xmls)
    {
        Console.WriteLine(item.Id.ToString() + " " + item.CreationDate.ToString());

    }
}





Console.WriteLine("Attempting to validate");
Console.WriteLine("Your OVAL XML doc is validated: " + new Validator().ISOVALXMLValidated(fullNameOfXMLfile: "res.xml"));

Console.ReadLine();