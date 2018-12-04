using SvenVissers.Steganography.Messages;
using System.Collections;
using System.Drawing;

namespace SvenVissers.Steganography.Helpers
{
    public static class Concealer
    {
        public static Bitmap Conceal(Bitmap image, Message message)
        {
            Bitmap concealedImage = ConcealMessageInBytes(image, message);

            return concealedImage;
        }

        private static Bitmap ConcealMessageInBytes(Bitmap image, Message message)
        {
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            message.CalculateNumberOfConcealBits(imageWidth, imageHeight);

            Bitmap concealedImage = new Bitmap(image);

            BitArray binaryMessage = new BitArray(message.GetBinaryMessage());
            int bitshiftIndex = 0;

            for (int y = 0; y < imageHeight; y++)
            {
                for (int x = 0; x < imageWidth; x++)
                {
                    ConcealInPixel(x, y, message, binaryMessage, ref bitshiftIndex, ref concealedImage);
                }
            }

            return concealedImage;
        }

        private static void ConcealInPixel(int x, int y, Message message, BitArray binaryMessage, ref int bitshiftIndex, ref Bitmap image)
        {
            Color pixel = image.GetPixel(x, y);

            byte[] chanels = new byte[3];

            for (int c = 0; c < chanels.Length; c++)
            {
                switch (c)
                {
                    case 0:
                        chanels[c] = ConcealInPixelChanelByte(pixel.R, ref bitshiftIndex, message, binaryMessage);
                        break;
                    case 1:
                        chanels[c] = ConcealInPixelChanelByte(pixel.G, ref bitshiftIndex, message, binaryMessage);
                        break;
                    case 2:
                        chanels[c] = ConcealInPixelChanelByte(pixel.B, ref bitshiftIndex, message, binaryMessage);
                        break;
                }
            }
            Color newPixel = Color.FromArgb(chanels[0], chanels[1], chanels[2]);
            image.SetPixel(x, y, newPixel);
        }

        private static byte ConcealInPixelChanelByte(byte chanel, ref int bitshiftIndex, Message message, BitArray binaryMessage)
        {
            int byteIndex = bitshiftIndex / 8;
            int numberOfConcealedBits = (byteIndex < 2) ? 1 : message.NumberOfConcealBits;

            for (int i = 0; i < numberOfConcealedBits; i++)
            {
                if (bitshiftIndex >= binaryMessage.Length)
                {
                    break;
                }

                ByteHelper.StoreBitValueInByte(ref chanel, i, binaryMessage.Get(bitshiftIndex));
                bitshiftIndex++;
            }

            return chanel;
        }
    }
}
