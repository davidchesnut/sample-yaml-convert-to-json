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
            try
            {
                //read arguments
                if (args.Length != 4)
                {
                    PrintCommandLineHelp();
                    return;
                }
                string org = args[0];
                string jsonInFile = args[1];
                string jsonOutFile = args[2];
                GitHubManager.instance.authtoken = args[3];

         

                JSONFile json = new JSONFile();
                json.ImportJSONFile(jsonInFile);


                var biglist = GetSampleFileList(org);
                foreach (var item in biglist.Items)
                {
                    var sample = new CodeSample();
                    Console.WriteLine("Processing repo: " + item.Repository.Name);

                    //introduce a 1 second delay between each GitHub call to avoid spamming GitHub
                    Task.Run(async () =>
                    {
                        await Task.Delay(1000);
                        return;
                    }).Wait();

                    sample.ImportYAMLFromGitHubFile(org, item.Repository.Name, item.Path);
                    
                    var sampleJson = sample.GetJSONObject();
                    if (sampleJson!=null) json.AddSampleToJSON(sampleJson);
                }
                
                json.SaveToFile(jsonOutFile);

                return;

            }
            catch (Exception e)
            {
                //For now errors will bubble up to here and get dropped to the console.
                Console.WriteLine(e.Message);
            }

        }
        static void PrintCommandLineHelp()
        {
            Console.WriteLine("Enter the following arguments to scan sample yaml to build a JSON file:");
            Console.WriteLine("- org: The GitHub organization to scan, such as officedev.");
            Console.WriteLine("- jsonInFile: The JSON file to read in and update.");
            Console.WriteLine("- jsonOutFile: Where to output the updated JSON.");
            Console.WriteLine("- authtoken: A GitHub public token to access with.");
            Console.WriteLine("\nExample:");
            Console.WriteLine(@"sample-yaml-to-json.exe officedev c:\projects\samples.json c:\projects\sample-new.json 15a7348cbd71cd63a6730d6f91bf8d9eaa32624a");
        }

        static SearchCodeResult GetSampleFileList(string org)
        {
            try
            {
                //string searchText = "{FileName = \"sample.yml\",User = \"officedev\",Extension = \"yml\"}";
                SearchCodeRequest sr = new SearchCodeRequest("YamlMime:Sample");
                // sr.FileName = "sample.yml";
                sr.Extension = "yml";
                sr.Organization = "officedev";

                sr.User = org;
                return GitHubManager.instance.client.Search.SearchCode(sr).Result;
            }
            catch (Exception e)
            {
                throw new Exception("Error searching GitHub: " + e.Message);
            }
            
        }

    

        
        
    }
}
