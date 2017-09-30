using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceInterface.Library.WSDL
{ 
    class WSDLTypeInformation
    {
        public string Name { get; set; }
        public List<WSDLType> Types { get; set; } 
    }
}
