using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebServiceInterface.Library
{
    class Parameter
    {
        public string type { get; set; }
        public string name { get; set; }
    }
      
    class Method
    {
        public string name { get; set; }
        public Parameter[] parameters { get; set; }
    }

    class ServiceInfo
    {
        public string name { get; set; }
        public string url { get; set; }
        public Method[] methods { get; set; }
    }


    class LibraryManager
    {
        private string _rawContent;
        private ServiceInfo[] allInfos;

        public LibraryManager(string fileName)
        {
            _rawContent = File.ReadAllText(fileName);
            allInfos = JsonConvert.DeserializeObject<ServiceInfo[]>(_rawContent);
        }

        public ServiceInfo getService(string name)
        {
            ServiceInfo result = null;

            foreach(ServiceInfo current in allInfos)
            {
                if (current.name == name)
                {
                    result = current;
                    break;
                } 
            }

            return result;
        }
    }
}
