using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patchTuesVisualStudioToXML.Parser.models
{
    public class MsrcUpdate
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context { get; set; }
        [JsonProperty(PropertyName = "value")]
        public List<MsrcUpdateValue> msrcUpdateValues { get; set;}
        public MsrcUpdate()
        {

        }
    }

    public class MsrcUpdateValue
    {
        public string ID { get; set; }
        public string Alias { get; set; }
        public string DocumentTitle { get; set; }
        public string Severity { get; set; }
        public DateTime InitialReleaseDate { get; set; }
        public DateTime CurrentReleaseDate { get; set; }
        public string CvrfUrl { get; set; }
        public MsrcUpdateValue()
        {

        }
    }
}
