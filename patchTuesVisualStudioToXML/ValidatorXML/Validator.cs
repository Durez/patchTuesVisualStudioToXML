using System.Xml.Linq;
using System.Xml.Schema;

namespace patchTuesVisualStudioToXML.Validator
{
    internal class Validator
    {
        /// <summary>
        /// XML file validation check method
        /// </summary>
        /// <param name="schemaTargetNameSpace">The schema targetNamespace property, or null to use the targetNamespace specified
        //     in the schema</param>
        /// <param name="schemaURI">The URL that specifies the schema to load.</param>
        /// <param name="fullNameOfXMLfile">Name of XML file in the root directory</param>
        /// <returns>Return boolean answer can XML file validate on XSD schema</returns>
        internal bool ISOVALXMLValidated(string schemaTargetNameSpace = "http://oval.mitre.org/XMLSchema/oval-common-5", string schemaURI = "Resourses/oval-common-schema.xsd", string fullNameOfXMLfile = "sample.xml")
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(schemaTargetNameSpace, schemaURI);

            XDocument generatedXML = XDocument.Load(Directory.GetCurrentDirectory() + "/Resourses/" +  fullNameOfXMLfile);
            bool errors = false;
            generatedXML.Validate(schemas, (o, e) =>
            {
                Console.WriteLine(e.Message);
                errors = true;
            });
            return !errors;
        }
        
    }
}
