﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceInterface.Library.WSDL
{
    class Message
    {
        public string name;
        public MessageType type;
        public List<Part> parts;

        public Message(string name, MessageType type)
        {
            this.name = name;
            this.type = type;
        }
    }

}
