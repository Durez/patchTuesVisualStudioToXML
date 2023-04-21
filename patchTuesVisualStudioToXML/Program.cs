using patchTuesVisualStudioToXML.Validator;

Console.WriteLine("Attempting to validate");
Console.WriteLine("custOrdDoc {0}", Validator.XMLISValidated() ? "did not validate" : "validated"); 
Console.ReadLine();