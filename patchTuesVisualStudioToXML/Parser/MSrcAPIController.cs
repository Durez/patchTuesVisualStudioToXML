﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Text.Json;
using patchTuesVisualStudioToXML.Parser.models;
using System.Xml;
using System.Globalization;
using patchTuesVisualStudioToXML.Parser.models.cvrfXMLmodel;
using System.Xml.Serialization;

namespace patchTuesVisualStudioToXML.Parser
{
    internal class MSrcAPIController
    {

        private HttpClient httpClient = new HttpClient();
        /// <summary>
        /// Getting url from MSRC
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GETRawCvrfURLAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync(string.Format(@"https://api.msrc.microsoft.com/cvrf/v2.0/Updates('{0}-{1}')", DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MMM", new CultureInfo("en-US"))));
            if (response.IsSuccessStatusCode)
            {
                string rawResponse = await response.Content.ReadAsStringAsync();
                MsrcUpdate res = JsonConvert.DeserializeObject<MsrcUpdate>(rawResponse);
       
                return res.msrcUpdateValues[0].CvrfUrl;
            }
            else throw new Exception(response.StatusCode.ToString() + ' ' + response.RequestMessage.ToString());
        }




        public async Task<Cvrfdoc> GETXMLCvrfAsync(string url)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string rawResponse = await response.Content.ReadAsStringAsync();
                XmlSerializer serializer = new XmlSerializer(typeof(Cvrfdoc));
                using (StringReader reader = new StringReader(rawResponse))
                    return (Cvrfdoc)serializer.Deserialize(reader);
            }
            else throw new Exception(response.StatusCode.ToString());
        }
    }
}
