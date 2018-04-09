using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Newtonsoft.Json;

namespace sampl_yaml_to_json
{
    /// <summary>
    /// Represents and handles operations for working with code sample properties including importing and exporting in YAML/JSON formats
    /// </summary>
    class CodeSample
    {
        private const string LASTMODIFIEDBY = "o365devx@microsoft.com";
        private const string RECOMMENDED = "False";

        private SampleYamlVT sampleYAML=null;
        private SampleJsonVT sampleJSON = null;

        /// <summary>
        /// imports YAML from a file on GitHub and converts to internal representations to store both YAML and JSON for future operations.
        /// </summary>
        /// <param name="org">Name of the org on GitHub (such as "OfficeDev")</param>
        /// <param name="repoName">Name of the repo on GitHub (such as "Add-in-Sample")</param>
        /// <param name="path">Path name of the file on GitHub (such as "/sample.yml")</param>
        public void ImportYAMLFromGitHubFile(string org, string repoName, string path)
        {
            //TODO this should throw an error if the file is not found, or does not have correct YAML
           // string testContent = "### YamlMime:Sample\nsample:\r\n- name: 'Skype for Business: Healthcare app'\r\n  path: HealthcareApp\r\n  description: Skype for Android Healthcare app sample.\r\n  readme: ''\r\n  generateZip: FALSE\r\n  isLive: TRUE\r\n  technologies:\r\n  - Office Add-in\r\n  azureDeploy: ''\r\n  author: JohnAustin-MSFT\r\n  platforms: []\r\n  languages:\r\n  - Java\r\n  extensions:\r\n    products:\r\n    - Skype for Business\r\n    scenarios: []\r\n";

            //get file contents.
            var repoSample = GitHubManager.instance.client.Repository.Content.GetAllContents(org, repoName, path).Result;
            var content = repoSample[0].Content;

            //deserialize YAML to internal object representation
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            StringReader sr = new StringReader(content);
            sampleYAML = deserializer.Deserialize<SampleYamlVT>(sr);
           

            //Also store an internal JSON object representation
            sampleJSON = ConvertYamlToJsonObject(sampleYAML);
            sampleJSON.Url = "https://github.com/" + org + "/" + repoName;
        }

        /// <summary>
        /// Returns a JSON string representing the sample item properties
        /// </summary>
        /// <returns>The JSON string representing the sample. Null if no sample has been imported.</returns>
        public string GetJSON()
        {
            if (sampleYAML != null)
                return JsonConvert.SerializeObject(sampleYAML);
            else
                return null;
        }

        public SampleJsonVT GetJSONObject()
        {
            return sampleJSON;
        }

        /// <summary>
        /// Converts a Yaml value type to a Json value type
        /// </summary>
        /// <param name="sampleYaml">A YAML value type</param>
        /// <returns>A JSON value type</returns>
        private SampleJsonVT ConvertYamlToJsonObject(SampleYamlVT sampleYaml)
        {
            var sampleJson = new SampleJsonVT
            {
                DateCreated = DateTime.Now.ToLongDateString(),
                Description = sampleYAML.sample[0].description,
                Id = Guid.Empty,//todo this is either generated, or need to match with an existing one in json file passed in for comparison.
                Languages = sampleYaml.sample[0].languages,
                LastModifiedBy = LASTMODIFIEDBY,//todo
                LastModifiedDate = DateTime.Now.ToLongDateString(),//todo
                Products = sampleYaml.sample[0].products,
                Recommended = RECOMMENDED,
                Technologies = sampleYaml.sample[0].technologies,
                Thumbnail = "",//todo
                Title = sampleYaml.sample[0].name,
                Url = sampleYaml.sample[0].path//todo add http protocol
            };
            return sampleJson;
        }
    }

    /// <summary>
    /// Value type representation of YAML spec from a sample item
    /// </summary>
    internal class SampleYamlVT
    {
        /// <summary>
        /// The YAML spec begins with an array of samples (although it is only ever 1 item)
        /// </summary>
        public InnerSample[] sample
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Value type representation of YAML spec of an entry in the sample array.
    /// </summary>
    public class InnerSample
    {
        public string sample { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string[] technologies { get; set; }
        public string author { get; set; } //use this as last modified by?
        public string[] languages { get; set; }
        public string[] products { get; set; }
        //yaml entries that we don't use. //todo see if these can be excluded by the yaml deserializer
        public string path { get; set; }
        public string filename { get; set; }
        public string FullPathFileName { get; set; }
        public string zone { get; set; }
        public string relPath { get; set; }
        public string readme { get; set; }
        public string generateZip { get; set; }
        public string isLive { get; set; }
        public string azureDeploy { get; set; }
        public string[] platforms { get; set; }
        public Extensions extensions { get; set; }
    }

    /// <summary>
    /// Value type representation of YAML spec extension array item
    /// </summary>
    public class Extensions
    {
        public string[] scenarios { get; set; }
        public string[] products { get; set; }
    }

    public class SampleJsonFileVT
    {
        public List<SampleJsonVT> allSamples;
    }

    /// <summary>
    /// Value type representation of JSON spec version of a sample
    /// </summary>
    public class SampleJsonVT
    {
        public Guid Id;
        public string Recommended;
        public string Title;
        public string Description;
        public string DateCreated;
        public string LastModifiedDate;
        public string LastModifiedBy;
        public string Thumbnail; //unused for now
        public string Url; //"https://github.com/OfficeDev/Orky"
        public string[] Technologies { get; set; }
        public string[] Languages { get; set; }
        public string[] Products { get; set; }
    }
}
