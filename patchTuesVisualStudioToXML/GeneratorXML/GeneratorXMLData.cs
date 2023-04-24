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
        private const string rawIDstring = "oval:en.ovalxmlgen.win";
        private const string objectRawStrID = rawIDstring + ":obj:";
        private const string definitionRawStrID = rawIDstring + ":def:";
        private const string testRawStrID = rawIDstring + ":tst:";
        private const string stateRawStrID = rawIDstring + ":ste:";
        private const string variableRawStrID = rawIDstring + ":var:";

        private static readonly List<string> platformsConst = new List<string>()
                    { "Microsoft Windows 7", "Microsoft Windows 8.1", "Microsoft Windows 10", "Microsoft Windows 11",
                        "Microsoft Windows Server 2008", "Microsoft Windows Server 2008 R2", "Microsoft Windows Server 2012",
                        "Microsoft Windows Server 2012 R2", "Microsoft Windows Server 2016", "Microsoft Windows Server 2019"};
        private int lastDefID = 11111;
        private int lastRegistryTestID = 22222;
        private int lastRegistryObjID = 1;
        private int lastDefrefID = 77777;
        private int lastObjID = 55555;
        private int lastStateID = 33333;
        private int lastVarID = 44444;
        
        private Cvrfdoc cvrfdoc {get;}
        public OvalDefinitions resultOVALXML { get; set; }
        public GeneratorXMLData()
        {

        }
        public GeneratorXMLData(OvalDefinitions sampleDoc)
        {
            resultOVALXML = sampleDoc;
        }
        public OvalDefinitions GenerateXMLData(Cvrfdoc cvrfdoc)
        {
            resultOVALXML.generator = new GeneratorTAG(schemaVersion, "OVALXMLgen by AGA");

            List<Vulnerability> VulnerabListFiltredByProj = GetVulnerabilities(cvrfdoc);

            resultOVALXML.tests.registryTest = new List<RegistryTest>();
            resultOVALXML.objects.registryObject = new List<RegistryObject>();

            //create uninstall registry object
            resultOVALXML.objects.registryObject.Add(CreateUninstallRegistryObject());

            //tag defenitions
            resultOVALXML.definitions.definitionsList = new List<Definition>();
            foreach (Vulnerability vulnerability in VulnerabListFiltredByProj)
            {
                //list of ref docs (inventory)
                List<Definition> refInventoryDef = new List<Definition>();

                //root of def
                Definition definition = new Definition();
                definition.@class = "vulnerability";
                definition.id = definitionRawStrID + lastDefID.ToString();
                definition.version = "1";

                //tag metadata
                List<string> products = new List<string>();
                //different prodID:years of versions VS
                Dictionary<string,string> visualStudioProdIDYearPairs = new Dictionary<string, string>();
                foreach (var branch in from branch in cvrfdoc.ProductTree.Branch.BranchsList
                                       where branch.Name == "Developer Tools"
                                       select branch)
                {
                    foreach (var productID in vulnerability.ProductStatuses.Status.ProductIDsList)
                    {
                        foreach (var (fullProductName, indexOfYear, nameOfProduct) in from fullProductName in branch.FullProductNamesList
                                                                                      where fullProductName.ProductID == productID
                                                                                      let indexOfYear = fullProductName.Text.IndexOf("Microsoft Visual Studio ") + 24
                                                                                      let nameOfProduct = "Microsoft Visual Studio " + fullProductName.Text.Substring(indexOfYear, 4)
                                                                                      select (fullProductName, indexOfYear, nameOfProduct))
                        {
                            if (!products.Contains(nameOfProduct))
                            {
                                products.Add(nameOfProduct);
                                visualStudioProdIDYearPairs.Add(productID, fullProductName.Text.Substring(indexOfYear, 4));
                            }
                            break;
                        }
                    }

                    break;
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
                    criteriaMID.extendDefinition = GenerateTagExtendDefinitionVulnerability(nameOfProduct + " is installed", definitionRawStrID + lastDefrefID.ToString());
                    //add after root definition
                    refInventoryDef.Add(GenerateTagDefinitionInventory(lastDefrefID, nameOfProduct, ProdIDYearPair.Value, lastRegistryTestID.ToString()));
                    lastDefrefID++;
                    
                    CreateExtendedDefinitionHierarchyTreeYearVers(nameOfProduct);


                    Criteria criteriaBOT = new Criteria();
                    criteriaBOT.criterionsList = new List<Criterion>();

                    foreach (var remediation in vulnerability.Remediations.RemediationsList)
                    {
                        string lowerVersion = remediation.FixedBuild.Substring(0, remediation.FixedBuild.LastIndexOf('.'));
                        string upperVersion = remediation.FixedBuild;
                        if (remediation.URL.Contains(ProdIDYearPair.Value) || remediation.ProductIDList[0] == ProdIDYearPair.Key)
                        {
                            string comment = string.Format("Check if the version of Visual Studio is greater than or equal {0} and less than {1}", lowerVersion, upperVersion);
                            criteriaBOT.criterionsList.Add(new Criterion(testRawStrID + lastRegistryTestID.ToString(), comment));
                            //add test tag
                            string objID = objectRawStrID + lastObjID.ToString();

                            CreateRegistryTestForProductVers(nameOfProduct, lowerVersion, upperVersion, comment, objID);

                            lastRegistryTestID++;
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

                }


                definition.metadata = metadata;
                definition.criteria = criteriaTOP;
                resultOVALXML.definitions.definitionsList.Add(definition);
                resultOVALXML.definitions.definitionsList.AddRange(from def in refInventoryDef
                                                                   select def);
                lastDefID++;
                break;
            }
            return resultOVALXML;
        }

        private void CreateExtendedDefinitionHierarchyTreeYearVers(string nameOfProduct)
        {
            RegistryTest t = CreateRegistryTestInstallYear(testRawStrID + lastRegistryTestID.ToString(),
                                                                "Check if " + nameOfProduct + " is installed",
                                                                objectRawStrID + lastRegistryObjID.ToString());
            if (!resultOVALXML.tests.registryTest.Contains(t))
                resultOVALXML.tests.registryTest.Add(t);
                                       

            resultOVALXML.objects.registryObject.Add(
                CreateRegistryObjectHolds(objectRawStrID + lastRegistryObjID.ToString(),
                                        "The registry holds " + nameOfProduct, stateRawStrID + lastStateID.ToString()));
            resultOVALXML.states.registryState.Add(
               CreateRegistryStateInstall(objectRawStrID + lastRegistryObjID.ToString(), "State matches if " + nameOfProduct + "is installed"));
            lastRegistryTestID++;
            resultOVALXML.variables.localVariable.Add(
                CreateLocalVariableRegProdKey(variableRawStrID + lastVarID.ToString(),
                                        "Full key path of " + nameOfProduct + " from uninstall registry key",
                                        objectRawStrID + lastRegistryObjID.ToString()));
            lastRegistryObjID++;
            resultOVALXML.objects.registryObject.Add(
                CreateRegistryObjectRegKey(objectRawStrID + lastRegistryObjID.ToString(),
                                        "Registry key for " + nameOfProduct,
                                        variableRawStrID + lastVarID.ToString()));
            lastVarID++;
        }

        private RegistryTest CreateRegistryTestInstallYear(string ID, string comment, string objectRefID)
        {
            ObjectTAG objectRefYear = new ObjectTAG(objectRefID);
            RegistryTest registryTest = new RegistryTest(ID, comment, null, objectRefYear);
            return registryTest;
        }

        private RegistryObject CreateRegistryObjectHolds(string id, string comment, string filterId)
        {

            SetTAG set = new SetTAG();
            set.objectReference = objectRawStrID + "99999";
            set.filter = new Filter() { action = "include", text = filterId };
            RegistryObject registryObject = new RegistryObject(id, comment, null, null, null, null, set: set);   

            return registryObject;
        }

        private RegistryState CreateRegistryStateInstall(string id, string comment)
        {
            RegistryState registryState = new RegistryState(id, comment);

            registryState.value = new ValueTAG() { operation = "pattern match", text = "^Visual Studio.*2022$" } ;
            return registryState;
        }

        private LocalVariable CreateLocalVariableRegProdKey(string id, string comment, string objComponentID)
        {
            ObjectComponent objectComponent = new ObjectComponent("key", objectRawStrID + objComponentID);
            LocalVariable localVariable = new LocalVariable(id, "1", comment, "string", objectComponent);
            return localVariable;
        }

        private RegistryObject CreateRegistryObjectRegKey(string id, string comment, string objComponentID)
        {
            Behaviors behaviors = new Behaviors("32_bit");
            KeyTAG keyTAG = new KeyTAG(id, "at least one");

            RegistryObject registryObject = new RegistryObject(id, comment, behaviors, "HKEY_LOCAL_MACHINE", keyTAG, "DisplayVersion");

            return registryObject;
        }




        private RegistryTest CreateRegistryTestForProductVers(string id, string comment, string objID, string lowerVersion, string upperVersion)
        {

            ObjectTAG objectTAG = new ObjectTAG(objectRawStrID + lastRegistryObjID.ToString());

            List<State> states = new List<State>();
            states.Add(CreateStateAndRegistryStateProdVers(lowerVersion));
            states.Add(CreateStateAndRegistryStateProdVers(upperVersion));

            RegistryTest registryTest = new RegistryTest(id, comment, states, objectTAG);
            return registryTest;
        }

        private State CreateStateAndRegistryStateProdVers(string version)
        {
            string stateref = stateRawStrID + lastStateID;
            RegistryState registryState = new RegistryState(new ValueTAG("less than", "version", version), "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows", stateref, "1", "State holds if the version is greater than or equal " + version);
            resultOVALXML.states.registryState.Add(registryState);
            lastStateID++;
            return new State(stateref);
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
                metadata.referencesList = new List<Reference>();
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

        public Definition GenerateTagDefinitionInventory(int id, string productName, string productYear, string registryTestid)
        {
            Definition definition = new Definition();
            definition.@class = "inventory";
            definition.id = definitionRawStrID + id;
            definition.version = "1";
            //metadata
            MetadataTAG metadata = GenerateTagMetadataInventorySample(productName, productYear);
            definition.metadata = metadata;
            definition.criteria = new Criteria();
            definition.criteria.criterionsList = new List<Criterion>();
            definition.criteria.criterionsList.Add(new Criterion(testRawStrID + registryTestid, "Check if " + productName + " is installed"));
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
            metadata.referencesList = new List<Reference>();
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


        private RegistryObject CreateUninstallRegistryObject()
        {
            RegistryObject registryObject = new RegistryObject(objectRawStrID + "99999", null, new Behaviors("32_bit"), "HKEY_LOCAL_MACHINE",
                new KeyTAG() { operation = "pattern match", text = "^Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\.*$" }, "DisplayName");
            return registryObject;
        }


    }
}
