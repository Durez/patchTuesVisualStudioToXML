using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using patchTuesVisualStudioToXML.GeneratorXML.models;
using System.Linq;

namespace patchTuesVisualStudioToXML.GeneratorXML
{
    public class GeneratorXMLData
    {
        private const string schemaVersion = "5.10.1";
        private const string rawIDstring = "oval:ru.altx-soft.win"; // "oval:en.ovalxmlgen.win";
        private const string objectRawStrID = rawIDstring + ":obj:";
        private const string definitionRawStrID = rawIDstring + ":def:";
        private const string testRawStrID = rawIDstring + ":tst:";
        private const string stateRawStrID = rawIDstring + ":ste:";
        private const string variableRawStrID = rawIDstring + ":var:";

        private readonly List<string> platformsConst = new List<string>()
                    { "Microsoft Windows 7", "Microsoft Windows 8.1", "Microsoft Windows 10", "Microsoft Windows 11",
                        "Microsoft Windows Server 2008", "Microsoft Windows Server 2008 R2", "Microsoft Windows Server 2012",
                        "Microsoft Windows Server 2012 R2", "Microsoft Windows Server 2016", "Microsoft Windows Server 2019"};
        private int lastDefID = 11111;
        private int lastRegistryTestID = 22222;
        private int lastRegistryObjID = 111;
        private int lastDefrefID = 77777;
        private int lastStateID = 33333;
        private int lastVarID = 44444;
        
        public OvalDefinitions resultOVALXML { get; set; }

        public GeneratorXMLData(OvalDefinitions sampleDoc)
        {
            resultOVALXML = sampleDoc;
        }
        public OvalDefinitions GenerateXMLData(Cvrfdoc cvrfdoc)
        {
            resultOVALXML.Generator = new GeneratorTAG(schemaVersion, "OVALXMLgen by AGA");

            List<Vulnerability> VulnerabListFiltredByProj = GetVulnerabilities(cvrfdoc);

            resultOVALXML.Tests.RegistryTest = new List<RegistryTest>();
            resultOVALXML.Objects.RegistryObject = new List<RegistryObject>();

            //create uninstall registry object
            resultOVALXML.Objects.RegistryObject.Add(CreateUninstallRegistryObject());

            //tag defenitions
            resultOVALXML.Definitions.DefinitionsList = new List<Definition>();
            foreach (Vulnerability vulnerability in VulnerabListFiltredByProj)
            {
                //list of ref docs (inventory)
                List<Definition> refInventoryDef = new List<Definition>();

                //root of def
                Definition definition = new Definition();
                definition.Class = "vulnerability";
                definition.Id = definitionRawStrID + lastDefID.ToString();
                definition.Version = "1";

                //tag metadata
                List<string> products = new List<string>();
                //different prodID:years of versions VS
                Dictionary<string, string> visualStudioProdIDYearPairs = new Dictionary<string, string>();
                foreach (var branch in GetBranches(cvrfdoc))
                {
                    foreach (var productID in vulnerability.ProductStatuses.Status.ProductIDsList)
                    {
                        foreach (var (fullProductName, indexOfYear, nameOfProduct) in GetProductsInfo(branch, productID))
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
                criteriaTOP.CritersList = new List<Criteria>();
                if (visualStudioProdIDYearPairs.Count > 1)
                {
                    criteriaTOP.Operator = "OR";
                }
                foreach (var ProdIDYearPair in visualStudioProdIDYearPairs)
                {
                    Criteria criteriaMID = new Criteria();
                    criteriaMID.Comment = "VS " + ProdIDYearPair.Value;
                    string nameOfProduct = "Microsoft Visual Studio " + ProdIDYearPair.Value;
                    criteriaMID.ExtendDefinition = GenerateTagExtendDefinitionVulnerability(nameOfProduct + " is installed", definitionRawStrID + lastDefrefID.ToString());
                    //add after root definition
                    refInventoryDef.Add(GenerateTagDefinitionInventory(lastDefrefID, nameOfProduct, ProdIDYearPair.Value, lastRegistryTestID.ToString()));
                    lastDefrefID++;

                    CreateExtendedDefinitionHierarchyTreeYearVers(nameOfProduct);


                    Criteria criteriaBOT = new Criteria();
                    criteriaBOT.CriterionsList = new List<Criterion>();

                    foreach (var (lowerVersion, upperVersion, comment) in GetCriterionsInfo(vulnerability, ProdIDYearPair))
                    {
                        criteriaBOT.CriterionsList.Add(new Criterion(testRawStrID + lastRegistryTestID.ToString(), comment));
                        //add registry test tag
                        resultOVALXML.Tests.RegistryTest.Add(CreateRegistryTestForProductVers(testRawStrID + lastRegistryTestID.ToString(), comment, nameOfProduct, lowerVersion, upperVersion));
                        lastRegistryTestID++;
                    }

                    if (criteriaBOT.CriterionsList.Count > 1)
                    {
                        criteriaMID.CritersList = new List<Criteria>();
                        criteriaBOT.Comment = "Vulnerable versions";
                        criteriaBOT.Operator = "OR";
                        criteriaMID.CritersList.Add(criteriaBOT);
                    }
                    else
                    {
                        criteriaMID.CriterionsList = new List<Criterion>();
                        criteriaMID.CriterionsList.Add(criteriaBOT.CriterionsList[0]);
                    }
                    criteriaTOP.CritersList.Add(criteriaMID);
                    lastRegistryTestID++;
                    lastRegistryObjID++;
                }


                definition.Metadata = metadata;
                definition.Criteria = criteriaTOP;
                resultOVALXML.Definitions.DefinitionsList.Add(definition);
                resultOVALXML.Definitions.DefinitionsList.AddRange(from def in refInventoryDef
                                                                   select def);
                lastDefID++;
            }
            return resultOVALXML;
        }

        private IEnumerable<(string lowerVersion, string upperVersion, string comment)> GetCriterionsInfo(Vulnerability vulnerability, KeyValuePair<string, string> ProdIDYearPair)
        {
            return from remediation in vulnerability.Remediations.RemediationsList
                   let lowerVersion = remediation.FixedBuild.Substring(0, remediation.FixedBuild.LastIndexOf('.'))
                   let upperVersion = remediation.FixedBuild
                   where remediation.URL.Contains(ProdIDYearPair.Value) || remediation.ProductIDList[0] == ProdIDYearPair.Key
                   let comment = string.Format("Check if the version of Visual Studio is greater than or equal {0} and less than {1}", lowerVersion, upperVersion)
                   select (lowerVersion, upperVersion, comment);
        }

        private IEnumerable<(FullProductName fullProductName, int indexOfYear, string nameOfProduct)> GetProductsInfo(Branch branch, string productID)
        {
            return from fullProductName in branch.FullProductNamesList
                   where fullProductName.ProductID == productID
                   let indexOfYear = fullProductName.Text.IndexOf("Microsoft Visual Studio ") + 24
                   let nameOfProduct = "Microsoft Visual Studio " + fullProductName.Text.Substring(indexOfYear, 4)
                   select (fullProductName, indexOfYear, nameOfProduct);
        }

        private IEnumerable<Branch> GetBranches(Cvrfdoc cvrfdoc)
        {
            return from branch in cvrfdoc.ProductTree.Branch.BranchsList
                   where branch.Name == "Developer Tools"
                   select branch;
        }

        private void CreateExtendedDefinitionHierarchyTreeYearVers(string nameOfProduct)
        {
            RegistryTest t = CreateRegistryTestInstallYear(testRawStrID + lastRegistryTestID.ToString(),
                                                                "Check if " + nameOfProduct + " is installed",
                                                                objectRawStrID + lastRegistryObjID.ToString());
            if (!resultOVALXML.Tests.RegistryTest.Contains(t))
                resultOVALXML.Tests.RegistryTest.Add(t);
                                       

            resultOVALXML.Objects.RegistryObject.Add(
                CreateRegistryObjectHolds(objectRawStrID + lastRegistryObjID.ToString(),
                                        "The registry holds " + nameOfProduct, stateRawStrID + lastStateID.ToString()));
            resultOVALXML.States.RegistryState.Add(
               CreateRegistryStateInstall(stateRawStrID + lastStateID.ToString(), "State matches if " + nameOfProduct + " is installed"));
            lastStateID++;
            lastRegistryTestID++;
            resultOVALXML.Variables.LocalVariable.Add(
                CreateLocalVariableRegProdKey(variableRawStrID + lastVarID.ToString(),
                                        "Full key path of " + nameOfProduct + " from uninstall registry key",
                                        objectRawStrID + lastRegistryObjID.ToString()));
            lastRegistryObjID++;
            resultOVALXML.Objects.RegistryObject.Add(
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
            set.ObjectReference = objectRawStrID + "88888";
            set.Filter = new Filter() { Action = "include", Text = filterId };
            RegistryObject registryObject = new RegistryObject(id, comment, null, null, null, null, set: set);   

            return registryObject;
        }

        private RegistryState CreateRegistryStateInstall(string id, string comment)
        {
            RegistryState registryState = new RegistryState(id, comment);

            registryState.Value = new ValueTAG() { Operation = "pattern match", Text = "^Visual Studio.*2022$" } ;
            return registryState;
        }

        private LocalVariable CreateLocalVariableRegProdKey(string id, string comment, string objComponentID)
        {
            ObjectComponent objectComponent = new ObjectComponent("key", objComponentID);
            LocalVariable localVariable = new LocalVariable(id, "1", comment, "string", objectComponent);
            return localVariable;
        }

        private RegistryObject CreateRegistryObjectRegKey(string id, string comment, string objComponentID)
        {
            Behaviors behaviors = new Behaviors("32_bit");
            KeyTAG keyTAG = new KeyTAG(objComponentID, "at least one");

            RegistryObject registryObject = new RegistryObject(id, comment, behaviors, "HKEY_LOCAL_MACHINE", keyTAG, "DisplayVersion");

            return registryObject;
        }




        private RegistryTest CreateRegistryTestForProductVers(string id, string comment, string objID, string lowerVersion, string upperVersion)
        {

            ObjectTAG objectTAG = new ObjectTAG(objectRawStrID + lastRegistryObjID.ToString());

            List<State> states = new List<State>();
            states.Add(CreateStateAndRegistryStateProdVers(lowerVersion, "greater than or equal", "State holds if the version is greater than or equal "));
            states.Add(CreateStateAndRegistryStateProdVers(upperVersion, "less than", "State holds if the version is less than "));

            RegistryTest registryTest = new RegistryTest(id, comment, states, objectTAG);
            return registryTest;
        }

        private State CreateStateAndRegistryStateProdVers(string version, string @operator, string comment)
        {
            string stateref = stateRawStrID + lastStateID;
            RegistryState registryState = new RegistryState(new ValueTAG(@operator, "version", version), stateref, "1", comment + version);
            resultOVALXML.States.RegistryState.Add(registryState);
            lastStateID++;
            return new State(stateref);
        }



        public MetadataTAG GenerateTagMetadataVulnerability(string titleDescriprtion, List<string> platforms, List<string> products, string cveID, string family = "windows")
        {
            MetadataTAG metadata = new MetadataTAG();
            if (titleDescriprtion != null)
            {
                metadata.Title = titleDescriprtion;
                metadata.Description = titleDescriprtion;
            }
            else throw new Exception();

            if (platforms != null || products != null)
            {
                Affected affected = new Affected();
                if (platforms != null)
                    affected.PlatformsList = platforms;
                else
                {
                    affected.PlatformsList = platformsConst;

                }
                if (products != null)
                    affected.ProductsList = products;

                if (family != null) affected.Family = family;
                metadata.Affected = affected;
            }

            if (cveID != null)
            {
                metadata.ReferencesList = new List<Reference>();
                metadata.ReferencesList.Add(new Reference() { Source = "Microsoft", RefId = cveID, RefUrl = "https://msrc.microsoft.com/update-guide/vulnerability/" + cveID });
                metadata.ReferencesList.Add(new Reference() { Source = "CVE", RefId = cveID, RefUrl = "https://cve.mitre.org/cgi-bin/cvename.cgi?name=" + cveID });
            }
            return metadata;
        }

        public ExtendDefinition GenerateTagExtendDefinitionVulnerability(string comment, string def_ref)
        {
            ExtendDefinition extendDefinition = new ExtendDefinition();
            extendDefinition.Comment = comment;
            extendDefinition.DefinitionRef = def_ref;
            return extendDefinition;
        }

        public Definition GenerateTagDefinitionInventory(int id, string productName, string productYear, string registryTestid)
        {
            Definition definition = new Definition();
            definition.Class = "inventory";
            definition.Id = definitionRawStrID + id;
            definition.Version = "1";
            //metadata
            MetadataTAG metadata = GenerateTagMetadataInventorySample(productName, productYear);
            definition.Metadata = metadata;
            definition.Criteria = new Criteria();
            definition.Criteria.CriterionsList = new List<Criterion>();
            definition.Criteria.CriterionsList.Add(new Criterion(testRawStrID + registryTestid, "Check if " + productName + " is installed"));
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
            metadata.Title = productName + " is installed";
            metadata.Description = productName + " is installed";
            metadata.Affected = new Affected();
            metadata.Affected.Family = "windows";
            metadata.Affected.PlatformsList = (from item in platformsConst
                                               select item).ToList();
            metadata.Affected.ProductsList = new List<string>();
            metadata.Affected.ProductsList.Add(productName);
            metadata.ReferencesList = new List<Reference>();
            metadata.ReferencesList.Add(new Reference() { Source = "CPE", RefId = "cpe:/a:microsoft:visual_studio:" + productYear });
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
                throw new GeneratorValueExeption("VulnerabListFiltredByProj");
            }
            return VulnerabListFiltredByProj;
        }


        private RegistryObject CreateUninstallRegistryObject()
        {
            RegistryObject registryObject = new RegistryObject(objectRawStrID + "88888", null, new Behaviors("32_bit"), "HKEY_LOCAL_MACHINE",
                new KeyTAG() { Operation = "pattern match", Text = @"^Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\.*$" }, "DisplayName");
            return registryObject;
        }


    }
}
