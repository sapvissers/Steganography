using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SvenVissers.Steganography.Exceptions
{
    public class MessageNotFoundException : Exception
    {
        public MessageNotFoundException(int availableSpace, int messageSize) : base(String.Format("No message was found in image."))
        {
        }
    }
}
