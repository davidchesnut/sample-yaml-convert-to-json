using System;
using Octokit;
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
            string org = args[0];
            string jsonFile = args[1];
            GitHubManager.instance.authtoken = args[2];
            GetSampleFileList(org);

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

        static string[] GetSampleFileList(string org)
        {
            //string searchText = "{FileName = \"sample.yml\",User = \"officedev\",Extension = \"yml\"}";
            SearchCodeRequest sr = new SearchCodeRequest();
            sr.FileName = "sample.yml";
            sr.Extension = "yml";
            sr.User = org;
            SearchCodeResult searchResult = GitHubManager.instance.client.Search.SearchCode(sr).Result;
            return null;

        }

    }
}
