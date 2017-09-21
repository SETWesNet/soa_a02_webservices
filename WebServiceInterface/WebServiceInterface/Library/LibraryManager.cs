using System.IO;
using Newtonsoft.Json;

namespace WebServiceInterface.Library
{
    class LibraryManager
    {
        private string _rawContent;
        private WebService[] _services;

        public LibraryManager(string fileName)
        {
            _rawContent = File.ReadAllText(fileName);
            _services = JsonConvert.DeserializeObject<WebService[]>(_rawContent);
        }

        public WebService getService(string name)
        {
            WebService result = null;

            foreach(WebService service in _services)
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
            WebService service = getService(serviceName);

            return service.methods;
        }

    }
}
