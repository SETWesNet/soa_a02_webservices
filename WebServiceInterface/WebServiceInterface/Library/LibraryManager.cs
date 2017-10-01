using System.IO;
using Newtonsoft.Json;
using System.Linq;
using WebServiceInterface.Library.WSDL;
using System.Collections.Generic;

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

        public Method GetMethod(string serviceURL, string methodName)
        {
            Method result = null;

            WebService service = _services.FirstOrDefault(s => s.Url == serviceURL);

            if (service != null)
            {
                result = service.Methods.FirstOrDefault(m => m.Name == methodName);
            }

            return result;
        }

        public Parameter[] GetParameters(string serviceURL, string methodName)
        {
            Method method = GetMethod(serviceURL, methodName);
            return method.Parameters;
        }
        public Parameter GetParameter(string serviceURL, string methodName, string parameterName)
        {
            return _services.First(service => service.Url == serviceURL)
                .Methods.First(method => method.Name == methodName)
                .Parameters.First(parameter => parameter.Name == parameterName);
        }


        public void SetMethod(Method newMethod, string serviceUrl, string methodName)
        {
            //NOTE(c-jm): Have to use the for loop here because we are keeping track of which method to set.

            int methodIndex = 0;
            WebService service = null;

            foreach(WebService currentService in _services)
            {
                if (currentService.Url == serviceUrl)
                {
                    for(int j = 0; j < currentService.Methods.Count(); ++j)
                    {
                        if (currentService.Methods[j].Name == methodName)
                        {
                            methodIndex = j;
                            break;
                        }
                    }

                    service = currentService;
                    break;
                }
            }


            service.Methods.SetValue(newMethod, methodIndex);
        }

        public void MergeWithWSDL(WSDLInformation info)
        {
            foreach(WSDLOperation operation in info.Operations)
            {
                Method method = GetMethod(info.Port.Location, operation.Name);

                if (method == null)
                {
                    continue;
                }

                method.Response = operation.OutputTypeInformation.Types.Cast<WSDLType>().First();
                method.Namespace = info.Namespace;

                Parameter[] parameters = method.Parameters;

                foreach(Parameter parameter in parameters)
                {
                    WSDLType parameterTypeInfo = operation.InputTypeInformation.Types.Cast<WSDLType>().First(type => type.Name == parameter.Name);
                    parameter.Type = parameterTypeInfo.Type;
                }

                SetMethod(method, info.Port.Location, operation.Name);
            }
        }
    }
}
