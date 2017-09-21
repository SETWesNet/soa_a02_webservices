using System.IO;
using Newtonsoft.Json;

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
        private ServiceInfo[] _services;

        public LibraryManager(string fileName)
        {
            _rawContent = File.ReadAllText(fileName);
            _services = JsonConvert.DeserializeObject<ServiceInfo[]>(_rawContent);
        }

        public ServiceInfo getService(string name)
        {
            ServiceInfo result = null;

            foreach(ServiceInfo service in _services)
            {
                if (service.name == name)
                {
                    result = service;
                    break;
                } 
            }

            return result;
        }

        public Method[] getMethods(string serviceName)
        {
            ServiceInfo service = getService(serviceName);

            return service.methods;
        }
    }
}
