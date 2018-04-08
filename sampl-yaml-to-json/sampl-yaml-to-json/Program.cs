using System;
using Octokit;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sampl_yaml_to_json
{
    class Program
    {

        static void Main(string[] args)
        {
            //read arguments
            if  (args.Length!=3)
            {
                PrintCommandLineHelp();
                return;
            }

            //testtest
            var sample = new CodeSample();
            sample.ImportYAMLFromGitHubFile("officedev", "Excel-Add-in-WoodGrove-Expense-Trends", "/Excel-Add-in-WoodGrove-Expense-Trends.yml");
            var json = sample.GetJSON();

            return;

            //testtest



            string org = args[0];
            string jsonFile = args[1];
            GitHubManager.instance.authtoken = args[2];
            SearchCodeResult sr = GetSampleFileList(org);
            foreach(Octokit.SearchCode sc in sr.Items)
            {
                //var yaml = ImportYaml(sc,org);
                //SampleItem sample = ConvertYamlToObject(yaml);
            }

        }
        static void PrintCommandLineHelp()
        {
            Console.WriteLine("Enter the following arguments to scan sample yaml to build a JSON file:");
            Console.WriteLine("- org: The GitHub organization to scan, such as officedev.");
            Console.WriteLine("- jsonfile: The name of the JSON file to create.");
            Console.WriteLine("- authtoken: A GitHub public token to access with");
            Console.WriteLine("\nExample:");
            Console.WriteLine("sample-yaml-to-json.exe 'officedev' 'samples.JSON'");
        }

        static SearchCodeResult GetSampleFileList(string org)
        {
            //string searchText = "{FileName = \"sample.yml\",User = \"officedev\",Extension = \"yml\"}";
            SearchCodeRequest sr = new SearchCodeRequest();
            sr.FileName = "sample.yml";
            sr.Extension = "yml";
            sr.User = org;
            return GitHubManager.instance.client.Search.SearchCode(sr).Result;
        }

        

        static YamlStream ImportYaml(SearchCode sc,string org)
        {
            string testContent = "### YamlMime:Sample\nsample:\r\n- name: 'Skype for Business: Healthcare app'\r\n  path: HealthcareApp\r\n  description: Skype for Android Healthcare app sample.\r\n  readme: ''\r\n  generateZip: FALSE\r\n  isLive: TRUE\r\n  technologies:\r\n  - Office Add-in\r\n  azureDeploy: ''\r\n  author: JohnAustin-MSFT\r\n  platforms: []\r\n  languages:\r\n  - Java\r\n  extensions:\r\n    products:\r\n    - Skype for Business\r\n    scenarios: []\r\n";
            
            string content = testContent;

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            StringReader sr = new StringReader(testContent);
                var sample = deserializer.Deserialize<SampleYamlVT>(sr);
           // SampleJsonVT sj = ConvertYamlToJsonObject(sample);

//            string output = JsonConvert.SerializeObject(sj);

            var yaml = new YamlStream();
            //get file contents.

            // var repoSample = GitHubManager.instance.client.Repository.Content.GetAllContents(org, sc.Repository.Name, sc.Path).Result;
            // var content = repoSample[0].Content; 

            //Only process samples correctly market with the YamlMime header/comment
            //todo log out when files are found that are not samples to improve the search to exclude them
            if (content.Contains("### YamlMime:Sample"))
            {
                using (TextReader tr = new StringReader(content))
              {

                yaml.Load(tr);

              }

            

            }
            if (yaml.Documents.Count == 0) return null;
            return yaml;
        }
    }
}
