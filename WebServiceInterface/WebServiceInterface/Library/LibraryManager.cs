using System.IO;
using Newtonsoft.Json;

namespace WebServiceInterface.Library
{
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

        public Method[] getAvailableMethods(string serviceName)
        {
            ServiceInfo service = getService(serviceName);

            return service.methods;
        }

    }
}
