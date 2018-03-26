using System;
using Newtonsoft.Json;

namespace sampl_yaml_to_json
{
    /// <summary>
    /// Value type that tepresents a repo sample data matching the YAML spec (but in C# object format)
    /// This also is used to convert to JSON so the JSON property attributes are used
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
    public class SampleItem
    {
        public InnerSample[] sample
        {
            get;
            set;
        }



    }

    public class Extensions
    {
        public string[] scenarios { get; set; }
        public string[] products { get; set; }
    }

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
