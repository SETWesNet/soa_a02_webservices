using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceInterface.Library.WSDL
{
    class WSDLType
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public WSDLType(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
