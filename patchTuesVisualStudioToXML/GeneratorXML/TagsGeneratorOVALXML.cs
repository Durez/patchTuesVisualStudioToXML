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
                    affected.platformsList = new List<string>()
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
            definition.version = "1";
            //metadata
            MetadataTAG metadata = GenerateTagMetadataInventorySample(productName, productYear);
            definition.metadata = metadata;
            definition.criteria = new Criteria();
            definition.criteria.criterionsList = new List<Criterion>();
            definition.criteria.criterionsList.Add(new Criterion("oval:en.ovalxmlgen.win:tst:" + testid, "Check if " + productName + " is installed"));
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
            metadata.affected = new Affected();
            metadata.affected.family = "windows";
            metadata.affected.platformsList = new List<string>();
            foreach (var item in platforms)
            {
                metadata.affected.platformsList.Add(item);
            }
            metadata.affected.productsList = new List<string>();
            metadata.affected.productsList.Add(productName);
            metadata.referencesList.Add(new Reference() { source = "CPE", ref_id = "cpe:/a:microsoft:visual_studio:" + productYear });
            return metadata;
        }


    }
}
