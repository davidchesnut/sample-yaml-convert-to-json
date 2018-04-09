using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace sampl_yaml_to_json
{
    /// <summary>
    /// Represents the JSON file to be updated. This class will read the file store it internally,
    /// and check when making updates that the correct sample is being updated.
    /// </summary>
    class JSONFile
    {
        private SampleJsonFileVT sample=null;
        /// <summary>
        /// Imports the existing JSON file into an internal representation
        /// </summary>
        /// <param name="path">full path and name of JSON file to import</param>
        public void ImportJSONFile(string path)
        {
            string jsonValue = File.ReadAllText(path);
            sample = JsonConvert.DeserializeObject<SampleJsonFileVT>(jsonValue);
        }

        /// <summary>
        /// add a sample to the file representation. This will check if the sample already exists, and if so just update it.
        /// </summary>
        /// <param name="sampleJSON">The new sample to add</param>
        public void AddSampleToJSON(SampleJsonVT sampleJSON)
        {
            //check if sample already exists
            int loc = IndexOf(sampleJSON.Url);
            if (loc >= -0)
            {
                //update the existing sample
                //Note: this should not set datecreated or ID or URL as these should never change

                //TODO This will not set the LastModifiedBy, LastModifiedDate, or Thumbnail for now
                sample.allSamples[loc].Description = sampleJSON.Description;
                sample.allSamples[loc].Languages = sampleJSON.Languages;
                sample.allSamples[loc].Products = sampleJSON.Products;
                sample.allSamples[loc].Recommended = sampleJSON.Recommended;
                sample.allSamples[loc].Technologies = sampleJSON.Technologies;
                sample.allSamples[loc].Title = sampleJSON.Title;
                sample.allSamples[loc].LastModifiedBy = sampleJSON.LastModifiedBy;
                sample.allSamples[loc].LastModifiedDate = DateTime.Now.ToLongDateString();
            }
            else
            {
                //create a new sample
                
                sample.allSamples.Add(sampleJSON);
            }
        }

        /// <summary>
        /// return index location of sample matching given url
        /// </summary>
        /// <param name="url">url to match on</param>
        /// <returns>index location in array, or -1 if not found</returns>
        private int IndexOf(string url)
        {
            if (sample == null) return -1;
            if (sample != null)
            {
                for (int i = 0; i < sample.allSamples.Count; i++)
                {
                    if (string.Equals(sample.allSamples[i].Url, url)) return i;
                }
            }
            return -1;
        }

        public bool isSampleinJSON(string url)
        {
            if (IndexOf(url) >= 0) return true;
            return false;
        }

        /// <summary>
        /// save JSON representation to a new file as JSON.
        /// </summary>
        /// <param name="path">full path and name of file to save to</param>
        public void SaveToFile(string path)
        {
            if (sample != null)
            {
                var jsonText = JsonConvert.SerializeObject(sample);
                File.WriteAllText(path, jsonText);
            }
        }
    }
}
