using System.Xml.Linq;
using System.Xml.Schema;

namespace patchTuesVisualStudioToXML.Validator
{
    static internal class Validator
    {
        /// <summary>
        /// XML file validation check method
        /// </summary>
        /// <param name="schemaTargetNameSpace">The schema targetNamespace property, or null to use the targetNamespace specified
        //     in the schema</param>
        /// <param name="schemaURI">The URL that specifies the schema to load.</param>
        /// <param name="nameOfXMLfile">Name of XML file in the root directory</param>
        /// <returns>Return boolean answer can XML file validate on XSD schema</returns>
        static internal bool XMLISValidated(string schemaTargetNameSpace = "http://oval.mitre.org/XMLSchema/oval-common-5", string schemaURI = "Validator/oval-common-schema.xsd", string nameOfXMLfile = "oval_ru.altx-soft.win_def_89847.xml")
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(schemaTargetNameSpace, schemaURI);

            XDocument generatedXML = XDocument.Load(Directory.GetCurrentDirectory() + '/' +  nameOfXMLfile);
            bool errors = false;
            generatedXML.Validate(schemas, (o, e) =>
            {
                Console.WriteLine(e.Message);
                errors = true;
            });
            return errors;
        }
        
    }
}
