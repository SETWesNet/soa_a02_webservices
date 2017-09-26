using System.IO;
using Newtonsoft.Json;
using System.Linq;

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
            return _services.Where(service => service.name == name).First();
        }

        public Method[] getAvailableMethods(string serviceName)
        {
            WebService service = getService(serviceName);

            return service.methods;
        }

    }
}
