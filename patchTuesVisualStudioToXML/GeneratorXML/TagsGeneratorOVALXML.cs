using patchTuesVisualStudioToXML.GeneratorXML.models;
using patchTuesVisualStudioToXML.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.GeneratorXML
{
    internal class TagsGeneratorOVALXML
    {
        private XmlSerializer serializer = new XmlSerializer(typeof(Oval_definitions));
        public TagsGeneratorOVALXML()
        {

        }

        public Oval_definitions LoadSample()
        {             
            //load sample with standart settings
            using (StreamReader reader = new StreamReader("sample.xml"))
            {
                var test = (Oval_definitions)serializer.Deserialize(reader);
                return test;
            }
        }

        public void WriteOVALXMLToFile(Oval_definitions resultdoc, string nameOfFile = "res")
        {
            Stream fs = new FileStream(nameOfFile + ".xml", FileMode.Create);
            XmlWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
            var ns = new XmlSerializerNamespaces();
            ns.Add("oval", "http://oval.mitre.org/XMLSchema/oval-common-5");
            ns.Add("schemaLocation", "http://oval.mitre.org/XMLSchema/oval-definitions-5 oval-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#windows windows-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#independent independent-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-common-5 oval-common-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#linux linux-definitions-schema.xsd http://oval.mitre.org/XMLSchema/oval-definitions-5#unix unix-definitions-schema.xsd");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            serializer.Serialize(writer, resultdoc, ns);
            Console.WriteLine("Writing OVAL XML to file complited.");
        }


        /// <summary>
        /// Create tag "generator" in XML using only requered params
        /// </summary>
        /// <param name="schema_version">Specifies the version of the OVAL Schema that the document has been written in and that should be used for validation</param>
        public GeneratorTAG GenerateTagGenerator(string schema_version)
        {
            GeneratorTAG generator = new GeneratorTAG();
            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            generator.schema_version = schema_version;
            generator.timestamp = timestamp;
            return generator;
        }
        /// <summary>
        /// Create tag "generator" in XML using requered and optional params
        /// </summary>
        /// <param name="product_name">Specifies the name of the application used to generate the file</param>
        /// <param name="product_version">Specifies the version of the application used to generate the file</param>
        /// <param name="schema_version">Specifies the version of the OVAL Schema that the document has been written in and that should be used for validation</param>
        public GeneratorTAG GenerateTagGenerator(string schema_version, string? product_name = null, string? product_version = null)
        {
            GeneratorTAG generator = GenerateTagGenerator(schema_version);
            if (product_name != null) generator.product_name = product_name;
            if (product_version != null) generator.schema_version = product_version;
            return generator;
        }

        public MetadataTAG GenerateTagMetadataVulnerability(string titleDescriprtion, List<string> platforms, List<string> products, string cveID, string family = "Windows")
        {
            MetadataTAG metadata = new MetadataTAG();
            if (titleDescriprtion != null)
            {
                metadata.title = titleDescriprtion;
                metadata.description = titleDescriprtion;
            }
            else throw new Exception();

            if (platforms != null || products != null) 
            { 
                Affected affected = new Affected();
                if (platforms != null)
                    affected.platformsList = platforms;
                else
                {
                    affected.platformsList =  new List<string>()
                    { "Microsoft Windows 7", "Microsoft Windows 8.1", "Microsoft Windows 10", "Microsoft Windows 11",
                        "Microsoft Windows Server 2008", "Microsoft Windows Server 2008 R2", "Microsoft Windows Server 2012",
                        "Microsoft Windows Server 2012 R2", "Microsoft Windows Server 2016", "Microsoft Windows Server 2019",
                        "Microsoft Windows Server 2022"};

                }
                if (products != null)
                        affected.productsList = products;

                if (family != null) affected.family = family;
                metadata.affected = affected; 
            }

            if (cveID != null) 
            {
                metadata.referencesList.Add(new Reference() { source = "Microsoft", ref_id = cveID, ref_url = "https://msrc.microsoft.com/update-guide/vulnerability/" + cveID });
                metadata.referencesList.Add(new Reference() { source = "CVE", ref_id = cveID, ref_url = "https://cve.mitre.org/cgi-bin/cvename.cgi?name=" + cveID });
            }
            return metadata;
        }


        public Criteria GenerateTagCriteriaTOP(List<Criteria> MIDcriteries)
        {
            Criteria criteria = new Criteria();
            if (MIDcriteries.Count > 1)
            {
                criteria.@operator = "OR";
            }
            foreach (var crit in MIDcriteries)
            {
                criteria.critersList.Add(crit);
            }
            return criteria;
        }
        public Criteria GenerateTagCriteriaMID(List<Criterion> criterions,ExtendDefinition extendDefinition, string comment)
        {
            Criteria criteria = new Criteria();
            criteria.critersList.Add(new Criteria());
            criteria.comment = comment;
            if (criterions.Count > 1)
            {
                criteria.critersList[0].@operator = "OR";
            }
            foreach (var crit in criterions)
            {
                criteria.critersList[0].criterionsList.Add(crit);
                criteria.comment = "Vulnerable versions";
            }
            return criteria;
        }


        public Criterion GenerateTagCriterion(string comment, string id)
        {
            Criterion criterion = new Criterion();
            criterion.testRef = "oval:en.ovalxmlgen.win:tst: " + id;
            criterion.comment = comment;
            return criterion;
        }
        public Criterion GenerateTagCriterionVulnerability(string lowerVersion, string upperVersion, string id)
        {
            Criterion criterion = new Criterion();
            criterion.testRef = "oval:en.ovalxmlgen.win:tst: " + id;
            criterion.comment = string.Format("Check if the version of Visual Studio is greater than or equal {0} and less than {1}", lowerVersion, upperVersion);
            return criterion;
        }
        public ExtendDefinition GenerateTagExtendDefinitionVulnerability(string comment, string def_ref) 
        {
            ExtendDefinition extendDefinition = new ExtendDefinition();
            extendDefinition.comment = comment;
            extendDefinition.definitionRef = def_ref;
            return extendDefinition;
        }
        /// <summary>
        /// Create inventory defenition tag
        /// </summary>
        /// <param name="def_ref">integer value of reference id</param>
        /// <param name="productName"></param>
        /// <param name="productYear"></param>
        /// <returns></returns>
        public Definition GenerateTagDefinitionInventorySample(int def_ref, string productName, string productYear, string testid)
        {
            Definition definition = new Definition();
            definition.@class = "inventory";
            definition.id = "oval:en.ovalxmlgen.win:def: " + def_ref;
            //metadata
            MetadataTAG metadata = GenerateTagMetadataInventorySample(productName, productYear);
            definition.metadata = metadata;
            definition.criteria.criterionsList.Add(GenerateTagCriterion("Check if " + productName + " is installed", testid));
            return definition;
        }


        /// <summary>
        /// Create MetadataTag for inventory defenition
        /// </summary>
        /// <param name="productName">name of product included year(Microsoft Windows Server *year*)</param>
        /// <param name="productYear">string value in YYYY format</param>
        /// <returns></returns>
        public MetadataTAG GenerateTagMetadataInventorySample(string productName, string productYear)
        {
            //metadata
            string[] platforms = new string[]
                    { "Microsoft Windows 7", "Microsoft Windows 8.1", "Microsoft Windows 10", "Microsoft Windows 11",
                        "Microsoft Windows Server 2008", "Microsoft Windows Server 2008 R2", "Microsoft Windows Server 2012",
                        "Microsoft Windows Server 2012 R2", "Microsoft Windows Server 2016", "Microsoft Windows Server 2019"};
            MetadataTAG metadata = new MetadataTAG();
            metadata.title = productName + " is installed";
            metadata.description = productName + " is installed";
            metadata.affected.family = "windows";
            foreach (var item in platforms)
            {
                metadata.affected.platformsList.Add(item);
            }
            metadata.affected.productsList.Add(productName);
            metadata.referencesList.Add(new Reference() { source = "CPE", ref_id = "cpe:/a:microsoft:visual_studio:" + productYear });
            return metadata;
        }

    }
}
