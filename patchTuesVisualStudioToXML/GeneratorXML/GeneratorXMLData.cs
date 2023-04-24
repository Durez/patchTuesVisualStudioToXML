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
        const string schemaVersion = "5.11";
        private Cvrfdoc cvrfdoc {get;}
        private TagsGeneratorOVALXML generatorTags { get; }
        public GeneratorXMLData()
        {
            generatorTags = new TagsGeneratorOVALXML();
        }
        public void GenerateXMLData(Cvrfdoc cvrfdoc)
        {
            int lastDefID = 12345;
            int lastTestID = 23456;

            Oval_definitions resultOVALXML = generatorTags.LoadSample();

            resultOVALXML.generator = generatorTags.GenerateTagGenerator(schemaVersion, "OVALXMLgen by AGA");

            List<Vulnerability> VulnerabListFiltredByProj = GetVulnerabilities(cvrfdoc); //TODO LINQ

            //tag defenitions
            resultOVALXML.definitions.definitionsList = new List<Definition>();
            foreach (Vulnerability vulnerability in VulnerabListFiltredByProj)
            {
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
                    criteriaMID.extendDefinition = generatorTags.GenerateTagExtendDefinitionVulnerability("Microsoft Visual Studio " + ProdIDYearPair.Value + " is installed", "oval:en.ovalxmlgen.win:def:" + lastDefID.ToString());
                    Criteria criteriaBOT = new Criteria();
                    criteriaBOT.criterionsList = new List<Criterion>();

                    foreach (var remediation in vulnerability.Remediations.RemediationsList)
                    {
                        string lowerVersion = remediation.FixedBuild.Substring(0, remediation.FixedBuild.LastIndexOf('.'));
                        string upperVersion = remediation.FixedBuild;
                        if (remediation.URL.Contains(ProdIDYearPair.Value) || remediation.ProductIDList[0] == ProdIDYearPair.Key)
                        {
                            criteriaBOT.criterionsList.Add(generatorTags.GenerateTagCriterionVulnerability(lowerVersion, upperVersion, lastTestID.ToString()));
                            
                            lastTestID++;
                        }
                    }
                    if (criteriaBOT.criterionsList.Count > 1)
                    {
                        criteriaMID.critersList = new List<Criteria>();
                        criteriaMID.critersList.Add(criteriaBOT);
                    }
                    else
                    {
                        criteriaMID.criterionsList = new List<Criterion>();
                        criteriaMID.criterionsList.Add(criteriaBOT.criterionsList[0]);
                    }
                    criteriaTOP.critersList.Add(criteriaMID);
                }


                definition.metadata = metadata;
                definition.criteria = criteriaTOP;
                resultOVALXML.definitions.definitionsList.Add(definition);

                lastDefID++;
            }
            
            generatorTags.WriteOVALXMLToFile(resultOVALXML, "test");




            generatorTags.WriteOVALXMLToFile(resultOVALXML);
        }

        private List<Vulnerability> GetVulnerabilities(Cvrfdoc cvrfdoc)
        {
            List<Vulnerability> VulnerabListFiltredByProj = new List<Vulnerability>();
            foreach (Vulnerability vulnerability in cvrfdoc.VulnerabilitiesList)
            {
                foreach (Note note in vulnerability.Notes.NotesList)
                {
                    if (note.Title == "Visual Studio") //TODO field for any platform/products
                    {
                        VulnerabListFiltredByProj.Add(vulnerability);
                        break;
                    }
                }
            }
            if (!VulnerabListFiltredByProj.Any())
            {
                
            }
            return VulnerabListFiltredByProj;
            
        }
    }
}
