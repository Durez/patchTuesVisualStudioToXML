using patchTuesVisualStudioToXML.GeneratorXML;
using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser
{
    public class ParserMSRCXMLtoOVALXML
    {
        public void GetProductData(string productname = "Visual Studio") 
        { 
            MSrcAPIController controller =  new MSrcAPIController();
            DateTime timestart = DateTime.Now;
            Console.WriteLine("Start using Microsoft API");
            Cvrfdoc doc =  controller.GETXMLCvrfAsync(controller.GETRawCvrfURLAsync().GetAwaiter().GetResult()).GetAwaiter().GetResult();
            string duration = (DateTime.Now - timestart).TotalSeconds.ToString("0.##");
            Console.WriteLine("Create CVRFXML Document is done. Duration: " + duration + " seconds");

            new GeneratorXMLData().GenerateXMLData(doc);




            Console.WriteLine("Writing OVAL XML to file complited.");
        }
    }
}
