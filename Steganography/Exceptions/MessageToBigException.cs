using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SvenVissers.Steganography.Exceptions
{
    public class MessageToBigException : Exception
    {
        public MessageToBigException(int availableSpace, int messageSize) : base(String.Format("Message is to big to fit inside this image. Message size: {0} bytes, Available image size: {1} bytes", messageSize, availableSpace))
        {
        }
    }
}
