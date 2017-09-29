using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceInterface.Library.WSDL
{
    class MessageContainer
    {
        public string Key { get; set; }
        public List<Message> Items;

        public MessageContainer(string key, List<Message> items)
        {
            this.Key = key;
            this.Items = items;
        }
    }
}
