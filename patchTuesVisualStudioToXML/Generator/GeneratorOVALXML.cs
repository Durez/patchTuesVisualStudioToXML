using patchTuesVisualStudioToXML.Generator.models;
using patchTuesVisualStudioToXML.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Generator
{
    internal static class GeneratorOVALXML
    {
        private static oval_definitions docXML = new oval_definitions();
        private static XmlSerializer serializer = new XmlSerializer(typeof(oval_definitions));
        private static void WriteXMLToFile(string nameOfFile = "res")
        {
            Stream fs = new FileStream(nameOfFile + ".xml", FileMode.Create);
            XmlWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
            var ns = new XmlSerializerNamespaces();
            ns.Add("oval", "http://oval.mitre.org/XMLSchema/oval-common-5");
            ns.Add("schemaLocation", "http://oval.mitre.org/XMLSchema/oval-definitions-5 oval-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#windows windows-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#independent independent-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-common-5 oval-common-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#linux linux-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#unix unix-definitions-schema.xsd");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            serializer.Serialize(writer, docXML,ns);
            Console.WriteLine("Writing XML to file complited.");
        }


        public static void LoadSample() 
        {
            using (StreamReader reader = new StreamReader("sample.xml"))
            {
                var test = (oval_definitions)serializer.Deserialize(reader);
                docXML = test;
            }
            WriteXMLToFile();
            Console.WriteLine("Loading Sample done.");
        }

        #region tagGenerator

        /// <summary>
        /// Create tag "generator" in XML using only requered params
        /// </summary>
        /// <param name="schema_version">Specifies the version of the OVAL Schema that the document has been written in and that should be used for validation</param>
        private static void GenerateTagGenerator(string schema_version)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            docXML.generator.schema_version = schema_version;
            docXML.generator.timestamp = timestamp;
        }
        /// <summary>
        /// Create tag "generator" in XML using requered and optional params
        /// </summary>
        /// <param name="product_name">Specifies the name of the application used to generate the file</param>
        /// <param name="product_version">Specifies the version of the application used to generate the file</param>
        /// <param name="schema_version">Specifies the version of the OVAL Schema that the document has been written in and that should be used for validation</param>
        private static void GenerateTagGenerator(string schema_version, string? product_name = null, string? product_version = null)
        {
            GenerateTagGenerator(schema_version);
            if (product_name != null) docXML.generator.product_name = product_name;
            if (product_version != null) docXML.generator.product_name = product_name;
        }
        #endregion

        #region tagDefenition

        #region tagMetadata
        private static Metadata GenerateTagMetadata(string title, string[] platforms, string[] products, string description, string cveID, string family = "Windows")
        {
            Metadata metadata = new Metadata();
            if (title != null) metadata.title = title;
            else throw new Exception();

            if (platforms != null || products != null) 
            { 
                Affected affected = new Affected();
                if (platforms != null)
                    foreach (string platform in platforms)
                    {
                        affected.platforms.Add(platform);
                    }
                else
                {
                    foreach (string platform in new string[] 
                    { "Microsoft Windows 7", "Microsoft Windows 8.1", "Microsoft Windows 10", "Microsoft Windows 11",
                        "Microsoft Windows Server 2008", "Microsoft Windows Server 2008 R2", "Microsoft Windows Server 2012",
                        "Microsoft Windows Server 2012 R2", "Microsoft Windows Server 2016", "Microsoft Windows Server 2019",
                        "Microsoft Windows Server 2022"})
                    {
                        affected.platforms.Add(platform);
                    }
                }
                

                if (products != null)
                    foreach (string product in products)
                    {
                        affected.products.Add(product);
                    }
                if (family != null) affected.family = family;
                metadata.affected = affected; 
            }

            if (cveID != null) 
            {
                metadata.reference.Add(new Reference() { source = "Microsoft", ref_id = cveID, ref_url = "https://msrc.microsoft.com/update-guide/vulnerability/" + cveID });
                metadata.reference.Add(new Reference() { source = "CVE", ref_id = cveID, ref_url = "https://cve.mitre.org/cgi-bin/cvename.cgi?name=" + cveID });
            }
            if (description != null) metadata.description = description;
            return metadata;
        }

        #endregion


        #region tagCriteria
        private static Criteria GenerateTagCriteriaTOP(List<Criteria> MIDcriteries)
        {
            Criteria criteria = new Criteria();
            if (MIDcriteries.Count > 1)
            {
                criteria.Operator = "OR";
            }
            foreach (var crit in MIDcriteries)
            {
                criteria.Criters.Add(crit);
            }
            return criteria;
        }
        private static Criteria GenerateTagCriteriaMID(List<Criterion> criterions,ExtendDefinition extendDefinition, string comment)
        {
            Criteria criteria = new Criteria();
            criteria.Comment = comment;
            if (criterions.Count > 1)
            {
                criteria.Criters[0].Operator = "OR";
            }
            foreach (var crit in criterions)
            {
                criteria.Criters[0].Criterions.Add(crit);
                criteria.Comment = "Vulnerable versions";
            }
            return criteria;
        }


        private static Criterion GenerateTagCriterion(string comment, string id)
        {
            Criterion criterion = new Criterion();
            criterion.TestRef = "oval:en.ovalxmlgen.win:tst: " + id;
            criterion.Comment = comment;
            return criterion;
        }
        private static Criterion GenerateTagCriterion(string lowerVersion, string upperVersion, string id)
        {
            Criterion criterion = new Criterion();
            criterion.TestRef = "oval:en.ovalxmlgen.win:tst: " + id;
            criterion.Comment = string.Format("Check if the version of Visual Studio is greater than or equal {0} and less than {1}", lowerVersion, upperVersion);
            return criterion;
        }
        private static ExtendDefinition GenerateTagExtendDefinition(string comment, string def_ref) 
        {
            ExtendDefinition extendDefinition = new ExtendDefinition();
            extendDefinition.Comment = comment;
            extendDefinition.DefinitionRef = "oval:en.ovalxmlgen.win:def: " + def_ref;
            return extendDefinition;
        }
        #endregion




        #endregion




        #region tagTests
        private static void GenerateTagTests(string schema_version, string? product_name = null, string? product_version = null)
        {
            
        }

        #endregion






        public static void CreateXML()
        {
            LoadSample();
            Console.WriteLine(new MSrcAPIController().GETRawCvrfURLAsync().GetAwaiter().GetResult());
            GenerateTagGenerator("5.11","OVALXMLgen by AGA");
            WriteXMLToFile("XMLGen");
            Console.WriteLine(docXML.ToString());



        }


    }
}
