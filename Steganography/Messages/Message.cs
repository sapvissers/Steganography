using SvenVissers.Steganography.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SvenVissers.Steganography.Messages
{
    public class Message
    {
        public byte NumberOfConcealBits { get; private set; }
        public MessageType MessageType { get; private set; }
        public byte[] BinaryMessageBody { get; private set; }
        public bool Success { get; private set; }

        private readonly byte prefix = 0x0f;
        private readonly byte suffix = 0x00;

        #region constructors
        public Message(string messageBody)
        {
            this.MessageType = MessageType.UTF8_TEXT;
            this.BinaryMessageBody = Encoding.UTF8.GetBytes(messageBody);
        }

        public Message(byte[] messageBody, MessageType messageType)
        {
            this.MessageType = MessageType;
            this.BinaryMessageBody = messageBody;
        }

        public Message(byte[] binaryMessage)
        {
            List<byte> binaryMessageList = new List<byte>();
            Success = false;

            for (int i = 0; i < binaryMessage.Length; i++)
            {
                byte @byte = binaryMessage[i];

                if (i == 0 && @byte != prefix)
                {
                    break;
                }

                if (i < 3)
                {
                    RevealHeader(@byte, i);
                    continue;
                }

                if (@byte == suffix)
                {
                    Success = true;
                    break;
                }

                binaryMessageList.Add(@byte);
            }

            BinaryMessageBody = binaryMessageList.ToArray();
        }
        #endregion

        #region public methods
        public void CalculateNumberOfConcealBits(int width, int height)
        {
            // Number of pixels in image times amount of color chanels minus 16 reserved bytes for prefix and numberOfConcealBits, using only one bit per color chanel for this data
            int availableSpace = width * height * 3 - 16;

            // Message size minus two bytes containing the prefix and numberOfConcealBits data
            int messageSize = GetBinaryMessage().Length - 2;

            if (availableSpace < messageSize)
            {
                throw new MessageToBigException(availableSpace, messageSize);
            }

            NumberOfConcealBits = Convert.ToByte(messageSize / (availableSpace / 8) + 1);
        }

        public byte[] GetBinaryMessage()
        {
            List<byte> message = new List<byte>();

            message.AddRange(GetHeaders());
            message.AddRange(BinaryMessageBody);
            message.Add(suffix);

            return message.ToArray();
        }
        #endregion

        #region private methods
        private byte[] GetHeaders()
        {
            List<byte> headerBytes = new List<byte>();

            headerBytes.Add(prefix);

            headerBytes.Add(NumberOfConcealBits);

            byte binaryFileType = Convert.ToByte(MessageType);
            headerBytes.Add(binaryFileType);

            return headerBytes.ToArray();
        }

        private void RevealHeader(byte @byte, int index)
        {
            switch (index)
            {
                case 1:
                    NumberOfConcealBits = @byte;
                    break;
                case 2:
                    MessageType = (MessageType)@byte;
                    break;
            }
        }
        #endregion
    }
}
