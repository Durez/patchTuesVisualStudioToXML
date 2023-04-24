using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using patchTuesVisualStudioToXML.GeneratorXML.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace patchTuesVisualStudioToXML.GeneratorXML
{
    public class GeneratorXMLData
    {
        private const string schemaVersion = "5.11";
        private static readonly List<string> platformsConst = new List<string>()
                    { "Microsoft Windows 7", "Microsoft Windows 8.1", "Microsoft Windows 10", "Microsoft Windows 11",
                        "Microsoft Windows Server 2008", "Microsoft Windows Server 2008 R2", "Microsoft Windows Server 2012",
                        "Microsoft Windows Server 2012 R2", "Microsoft Windows Server 2016", "Microsoft Windows Server 2019"};
        private int lastDefID = 11111;
        private int lastTestID = 22222;
        private int lastDefrefID = 77777;
        private int lastObjID = 55555;
        private int lastStateID = 33333;
        private int lastVarID = 44444;
        private XmlSerializer serializer = new XmlSerializer(typeof(Oval_definitions));
        private Cvrfdoc cvrfdoc {get;}
        public Oval_definitions resultOVALXML { get; set; }
        public GeneratorXMLData()
        {
            resultOVALXML = LoadSample();
        }
        public void GenerateXMLData(Cvrfdoc cvrfdoc)
        {
            resultOVALXML.generator = new GeneratorTAG(schemaVersion, "OVALXMLgen by AGA");

            List<Vulnerability> VulnerabListFiltredByProj = GetVulnerabilities(cvrfdoc);

            resultOVALXML.tests.registryTest = new List<RegistryTest>();

            //tag defenitions
            resultOVALXML.definitions.definitionsList = new List<Definition>();
            foreach (Vulnerability vulnerability in VulnerabListFiltredByProj)
            {
                //list of ref docs (inventory)
                List<Definition> refInventoryDef = new List<Definition>();

                //root of def
                Definition definition = new Definition();
                definition.@class = "vulnerability";
                definition.id = "oval:en.ovalxmlgen.win:def:" + lastDefID.ToString();
                definition.version = "1";


                //tag metadata
                List<string> products = new List<string>();
                //different prodID:years of versions VS
                Dictionary<string,string> visualStudioProdIDYearPairs = new Dictionary<string, string>();
                foreach (var branch in cvrfdoc.ProductTree.Branch.BranchsList)
                {
                    if (branch.Name == "Developer Tools")
                    {
                        foreach (var productID in vulnerability.ProductStatuses.Status.ProductIDsList)
                        {
                            
                            
                            
                            foreach (var fullProductName in branch.FullProductNamesList)
                            {
                                if (fullProductName.ProductID == productID)
                                {
                                    int indexOfYear = fullProductName.Text.IndexOf("Microsoft Visual Studio ") + 24;
                                    string nameOfProduct = "Microsoft Visual Studio " + fullProductName.Text.Substring(indexOfYear, 4);
                                    if (!products.Contains(nameOfProduct)) 
                                    {
                                        products.Add(nameOfProduct);
                                        visualStudioProdIDYearPairs.Add(productID,fullProductName.Text.Substring(indexOfYear, 4));
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
                MetadataTAG metadata = GenerateTagMetadataVulnerability(vulnerability.Title, null, products, vulnerability.CVE);

                //tag upper criteria 
                Criteria criteriaTOP = new Criteria();
                criteriaTOP.critersList = new List<Criteria>();
                if (visualStudioProdIDYearPairs.Count > 1)
                {
                    criteriaTOP.@operator = "OR";
                }
                foreach (var ProdIDYearPair in visualStudioProdIDYearPairs)
                {
                    Criteria criteriaMID = new Criteria();
                    criteriaMID.comment = "VS " + ProdIDYearPair.Value;
                    string nameOfProduct = "Microsoft Visual Studio " + ProdIDYearPair.Value;
                    criteriaMID.extendDefinition = GenerateTagExtendDefinitionVulnerability(nameOfProduct + " is installed", "oval:en.ovalxmlgen.win:def:" + lastDefrefID.ToString());
                    //add ref doc (inventory)
                    refInventoryDef.Add(GenerateTagDefinitionInventorySample(lastDefrefID, nameOfProduct, ProdIDYearPair.Value, lastTestID.ToString()));
                    //TODO test in inventory
                    
                    lastDefrefID++;
                    Criteria criteriaBOT = new Criteria();
                    criteriaBOT.criterionsList = new List<Criterion>();

                    foreach (var remediation in vulnerability.Remediations.RemediationsList)
                    {
                        string lowerVersion = remediation.FixedBuild.Substring(0, remediation.FixedBuild.LastIndexOf('.'));
                        string upperVersion = remediation.FixedBuild;
                        if (remediation.URL.Contains(ProdIDYearPair.Value) || remediation.ProductIDList[0] == ProdIDYearPair.Key)
                        {
                            string comment = string.Format("Check if the version of Visual Studio is greater than or equal {0} and less than {1}", lowerVersion, upperVersion);
                            criteriaBOT.criterionsList.Add(new Criterion("oval:en.ovalxmlgen.win:tst:" + lastTestID.ToString(), comment));
                            //add test tag
                            string objID = "oval:en.ovalxmlgen.win:obj:" + lastObjID.ToString();

                            CreateRegistryTestForPRoductVers(nameOfProduct, lowerVersion, upperVersion, comment, objID);

                            lastTestID++;
                        }
                    }
                    if (criteriaBOT.criterionsList.Count > 1)
                    {
                        criteriaMID.critersList = new List<Criteria>();
                        criteriaBOT.comment = "Vulnerable versions";
                        criteriaBOT.@operator = "OR";
                        criteriaMID.critersList.Add(criteriaBOT);
                    }
                    else
                    {
                        criteriaMID.criterionsList = new List<Criterion>();
                        criteriaMID.criterionsList.Add(criteriaBOT.criterionsList[0]);
                    }
                    criteriaTOP.critersList.Add(criteriaMID);
                    lastObjID++;
                    lastTestID++;
                    lastVarID++;
                }


                definition.metadata = metadata;
                definition.criteria = criteriaTOP;
                resultOVALXML.definitions.definitionsList.Add(definition);
                foreach (var def in refInventoryDef)
                    resultOVALXML.definitions.definitionsList.Add(def);
                lastDefID++;
            }
            
            WriteOVALXMLToFile(resultOVALXML, "test");




            WriteOVALXMLToFile(resultOVALXML);
        }

        private void CreateRegistryTestForPRoductVers(string nameOfProduct, string lowerVersion, string upperVersion, string comment, string objID)
        {
            var t = from registryObject in resultOVALXML.objects.registryObject
                    where registryObject.comment == "Registry key for " + nameOfProduct
                    select new { };

            if (t.Any())
                resultOVALXML.objects.registryObject.Add(CreateRegistryObjectKey(nameOfProduct, objID));

            List<State> states = new List<State>();
            string stateref = "oval:en.ovalxmlgen.win:ste:" + lastStateID;
            states.Add(new State(stateref));
            RegistryState registryState1 = new RegistryState(new ValueTAG("greater than or equal", "version", lowerVersion), "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows", stateref, "1", "State holds if the version is greater than or equal " + lastStateID);
            lastStateID++;
            stateref = "oval:en.ovalxmlgen.win:ste:" + lastStateID;
            states.Add(new State(stateref));
            RegistryState registryState2 = new RegistryState(new ValueTAG("less than", "version", upperVersion), "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows", stateref, "1", "State holds if the version is greater than or equal " + upperVersion);
            lastStateID++;

            ObjectTAG objectTAG = new ObjectTAG(objID);
            RegistryTest registryTest = new RegistryTest("oval:en.ovalxmlgen.win:tst:" + lastTestID.ToString(), comment, states, objectTAG);
            resultOVALXML.tests.registryTest.Add(registryTest);
        }

        private RegistryObject CreateRegistryObjectKey(string nameOfProduct, string objID)
        {
            string keyID = "oval:en.ovalxmlgen.win:var:" + lastVarID.ToString();
            KeyTAG keyTAG = new KeyTAG(keyID, "at least one");
            string comment = "Registry key for " + nameOfProduct;
            RegistryObject registryObject = new RegistryObject(objID, comment, new Behaviors("32_bit"), "HKEY_LOCAL_MACHINE", keyTAG, "DisplayVersion");

            CreateLocalVarsKey(keyID, "Full key path of " + nameOfProduct + " from uninstall registry key", lastTestID.ToString()); //TODO id ????



            return registryObject;
        }

        private void CreateLocalVarsKey(string id, string comment, string idRefObject, string version = "1", string datatype = "string")
        {
            var t = "Full key path of Microsoft Visual Studio 2022 from uninstall registry key";
            var idRefObjectFull = "oval:en.ovalxmlgen.win:obj:" + idRefObject;
            ObjectComponent objectComponent = new ObjectComponent("key", "oval:en.ovalxmlgen.win:obj:" + idRefObject);
            LocalVariable localVariable = new LocalVariable("oval:en.ovalxmlgen.win:var:" + id, version, comment, datatype, objectComponent);
            resultOVALXML.variables.localVariable.Add(localVariable);
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
                    affected.platformsList = platformsConst;

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

        public Definition GenerateTagDefinitionInventorySample(int id, string productName, string productYear, string testid)
        {
            Definition definition = new Definition();
            definition.@class = "inventory";
            definition.id = "oval:en.ovalxmlgen.win:def: " + id;
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
            MetadataTAG metadata = new MetadataTAG();
            metadata.title = productName + " is installed";
            metadata.description = productName + " is installed";
            metadata.affected = new Affected();
            metadata.affected.family = "windows";
            metadata.affected.platformsList = new List<string>();
            foreach (var item in platformsConst)
            {
                metadata.affected.platformsList.Add(item);
            }
            metadata.affected.productsList = new List<string>();
            metadata.affected.productsList.Add(productName);
            metadata.referencesList.Add(new Reference() { source = "CPE", ref_id = "cpe:/a:microsoft:visual_studio:" + productYear });
            return metadata;
        }

        private List<Vulnerability> GetVulnerabilities(Cvrfdoc cvrfdoc, string platformName = "Visual Studio")
        {
            List<Vulnerability> VulnerabListFiltredByProj = new List<Vulnerability>();
            foreach (var vulnerability in from Vulnerability vulnerability in cvrfdoc.VulnerabilitiesList
                                          from Note note in vulnerability.Notes.NotesList
                                          where note.Title == platformName
                                          select vulnerability)
                VulnerabListFiltredByProj.Add(vulnerability);


            if (!VulnerabListFiltredByProj.Any())
            {
                
            }
            return VulnerabListFiltredByProj;
        }



    }
}
