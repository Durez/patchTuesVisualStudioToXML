using patchTuesVisualStudioToXML.GeneratorXML;
using patchTuesVisualStudioToXML.GeneratorXML.models;
using patchTuesVisualStudioToXML.Parser;
using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using patchTuesVisualStudioToXML.Validator;

Console.WriteLine("Attempting to validate");
//Console.WriteLine("sample {0}", Validator.XMLISValidated() ? "did not validate" : "validated");
Cvrfdoc rawDoc = ParserMSRCXMLtoOVALXML.GetProductData();

OvalDefinitions result = new GeneratorXMLData(ParserMSRCXMLtoOVALXML.LoadSample()).GenerateXMLData(rawDoc);

ParserMSRCXMLtoOVALXML.WriteOVALXMLToFile(result, "test");
Console.WriteLine(new Validator().OVALXMLISValidate(fullNameOfXMLfile: "test.xml"));

Console.ReadLine();