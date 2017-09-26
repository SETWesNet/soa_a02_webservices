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

        public WebService[] Services
        {
            get
            {
                return _services;
            }
        } 

        public WebService GetService(string serviceURL)
        {
            return _services.First(service => service.Url == serviceURL);
        }

        public Method[] GetAvailableMethods(string serviceURL)
        {
            WebService service = GetService(serviceURL);

            return service.Methods;
        }

    }
}
