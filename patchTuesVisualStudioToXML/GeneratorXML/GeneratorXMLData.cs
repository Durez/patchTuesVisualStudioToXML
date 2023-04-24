using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using patchTuesVisualStudioToXML.GeneratorXML.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patchTuesVisualStudioToXML.GeneratorXML
{
    public class GeneratorXMLData
    {
        private const string schemaVersion = "5.11";
        private Cvrfdoc cvrfdoc {get;}
        private TagsGeneratorOVALXML generatorTags { get; }
        public Oval_definitions resultOVALXML { get; set; }
        public GeneratorXMLData()
        {
            generatorTags = new TagsGeneratorOVALXML();
            resultOVALXML = generatorTags.LoadSample();
        }
        public void GenerateXMLData(Cvrfdoc cvrfdoc)
        {
            int lastDefID = 11111;
            int lastTestID = 22222;
            int lastDefrefID = 77777;
            int lastObjID = 55555;
            int lastStateID = 33333;
            int lastVarID = 44444;

            

            resultOVALXML.generator = new GeneratorTAG(schemaVersion, "OVALXMLgen by AGA");

            List<Vulnerability> VulnerabListFiltredByProj = GetVulnerabilities(cvrfdoc); //TODO LINQ

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
                MetadataTAG metadata = generatorTags.GenerateTagMetadataVulnerability(vulnerability.Title, null, products, vulnerability.CVE);

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
                    criteriaMID.extendDefinition = generatorTags.GenerateTagExtendDefinitionVulnerability(nameOfProduct + " is installed", "oval:en.ovalxmlgen.win:def:" + lastDefrefID.ToString());
                    //add ref doc (inventory)
                    refInventoryDef.Add(generatorTags.GenerateTagDefinitionInventorySample(lastDefrefID, nameOfProduct, ProdIDYearPair.Value, lastTestID.ToString()));
                    lastTestID++;
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
                            RegistryObject registryObject = CreateRegistryObjectWithRefs(lastVarID, nameOfProduct, objID);

                            RegistryState registryState1 = new RegistryState(new ValueTAG("greater than or equal", "version", lowerVersion), "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows", "oval:en.ovalxmlgen.win:ste:" + lastStateID, "1", "State holds if the version is greater than or equal " + lastStateID);
                            lastStateID++;
                            RegistryState registryState2 = new RegistryState(new ValueTAG("less than", "version", upperVersion), "http://oval.mitre.org/XMLSchema/oval-definitions-5#windows", "oval:en.ovalxmlgen.win:ste:" + lastStateID, "1", "State holds if the version is greater than or equal " + upperVersion);
                            lastStateID++;


                            List<State> states = new List<State>();

                            RegistryTest registryTest = new RegistryTest("oval:en.ovalxmlgen.win:tst:" + lastTestID.ToString(), comment, states, objectTAG);

                            resultOVALXML.tests.registryTest.Add(registryTest);

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
                    lastVarID++;
                }


                definition.metadata = metadata;
                definition.criteria = criteriaTOP;
                resultOVALXML.definitions.definitionsList.Add(definition);
                foreach (var def in refInventoryDef)
                    resultOVALXML.definitions.definitionsList.Add(def);
                lastDefID++;
            }
            
            generatorTags.WriteOVALXMLToFile(resultOVALXML, "test");




            generatorTags.WriteOVALXMLToFile(resultOVALXML);
        }

        private RegistryObject CreateRegistryObjectWithRefs(int lastVarID, string nameOfProduct, string objID)
        {
            ObjectTAG objectTAG = new ObjectTAG(objID);
            KeyTAG keyTAG = new KeyTAG("oval:en.ovalxmlgen.win:var:" + lastVarID, "at least one");
            RegistryObject registryObject = new RegistryObject(objID, "Registry key for " + nameOfProduct, new Behaviors("32_bit"), "HKEY_LOCAL_MACHINE", keyTAG, "DisplayVersion");





            return registryObject;
        }

        private void CreateLocalVars(string id, string comment, string idRefObject, string version = "1", string datatype = "string")
        {
            var t = "Full key path of Microsoft Visual Studio 2022 from uninstall registry key";
            var idRefObjectFull = "oval:en.ovalxmlgen.win:obj:" + idRefObject;
            ObjectComponent objectComponent = new ObjectComponent("key", "oval:en.ovalxmlgen.win:obj:" + idRefObject);
            LocalVariable localVariable = new LocalVariable("oval:en.ovalxmlgen.win:var:" + id, version, comment, datatype, objectComponent);
            resultOVALXML.variables.localVariable.Add(localVariable);

        }



        private List<Vulnerability> GetVulnerabilities(Cvrfdoc cvrfdoc, string platformName = "Visual Studio")
        {
            List<Vulnerability> VulnerabListFiltredByProj = new List<Vulnerability>();
            foreach (Vulnerability vulnerability in cvrfdoc.VulnerabilitiesList)
            {
                foreach (Note note in vulnerability.Notes.NotesList)
                {
                    if (note.Title == platformName) //TODO field for any platform/products
                    {
                        VulnerabListFiltredByProj.Add(vulnerability);
                        break;
                    }
                }
            }

            /*foreach (var vulnerability in from Vulnerability vulnerability in cvrfdoc.VulnerabilitiesList
                                          from Note note in vulnerability.Notes.NotesList
                                          where note.Title == platformName//TODO field for any platform/products
                                          select vulnerability)
            {
                VulnerabListFiltredByProj.Add(vulnerability);
                break;
            }*/

            if (!VulnerabListFiltredByProj.Any())
            {
                
            }
            return VulnerabListFiltredByProj;
        }



    }
}
