using SvenVissers.Steganography.Exceptions;
using SvenVissers.Steganography.Messages;
using System.Collections.Generic;
using System.Drawing;

namespace SvenVissers.Steganography.Helpers
{
    public class Revealer
    {
        private Message message = null;
        private List<byte> binaryMessage = new List<byte>();

        private int pixelIndex = 0;
        private int totalBitsRevealed = 0;

        private int maxPixels;
        private int imageWidth;

        private bool fullMessageRevealed = false;

        private byte prefix = 0x00;
        private byte numberOfConcealedBits = 0x00;
        private byte nextByte = 0x00;

        public Message Reveal(Bitmap image)
        {
            maxPixels = image.Width * image.Height;
            imageWidth = image.Width;

            RevealFullMessage(image);

            message = new Message(binaryMessage.ToArray());

            return message;
        }

        private void RevealFullMessage(Bitmap image)
        {
            while (!fullMessageRevealed && pixelIndex < maxPixels)
            {
                Color pixel = image.GetPixel(pixelIndex % imageWidth, pixelIndex / imageWidth);

                RevealPixelData(pixel);

                pixelIndex++;
            }
        }

        private void RevealPixelData(Color pixel)
        {
            for (int c = 0; c < 3; c++)
            {
                byte channel = PixelHelper.GetChannelFromColorByIndex(pixel, (ColorIndex)c);

                if (totalBitsRevealed < 8)
                {
                    prefix = RevealPrefixByte(channel);
                }
                else if (prefix != Message.prefix)
                {
                    throw new MessageNotFoundException();
                }
                else if (totalBitsRevealed < 16)
                {
                    numberOfConcealedBits = RevealNumberOfConcealedBits(channel);
                }
                else
                {
                    for (int b = 0; b < numberOfConcealedBits; b++)
                    {
                        RevealMessageBody(channel, b);
                    }
                }
            }
        }

        private byte RevealPrefixByte(byte channel)
        {
            bool bit = ByteHelper.GetBitValueInByte(channel, 0);
            ByteHelper.StoreBitValueInByte(ref prefix, totalBitsRevealed % 8, bit);
            nextByte = prefix;
            totalBitsRevealed++;

            UpdateBinaryMessage();

            return prefix;
        }

        private byte RevealNumberOfConcealedBits(byte channel)
        {
            bool bit = ByteHelper.GetBitValueInByte(channel, 0);
            ByteHelper.StoreBitValueInByte(ref numberOfConcealedBits, totalBitsRevealed % 8, bit);
            nextByte = numberOfConcealedBits;
            totalBitsRevealed++;

            UpdateBinaryMessage();

            return numberOfConcealedBits;
        }

        private void RevealMessageBody(byte channel, int position)
        {
            bool bit = ByteHelper.GetBitValueInByte(channel, position);
            ByteHelper.StoreBitValueInByte(ref nextByte, totalBitsRevealed % 8, bit);
            totalBitsRevealed++;

            UpdateBinaryMessage();
        }

        private void UpdateBinaryMessage()
        {
            if (totalBitsRevealed % 8 == 0)
            {
                binaryMessage.Add(nextByte);

                if (nextByte == Message.suffix)
                {
                    fullMessageRevealed = true;
                }
            }
        }
    }
}
