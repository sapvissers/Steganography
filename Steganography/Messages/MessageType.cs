using System;
using System.Collections.Generic;
using System.Text;

namespace SvenVissers.Steganography.Messages
{
    public enum MessageType
    {
        BINARY,
        ASCII_TEXT,
        UNICODE_TEXT,
        UTF7_TEXT,
        UTF8_TEXT,
        UTF32_TEXT,
        IMAGE,
    }
}
