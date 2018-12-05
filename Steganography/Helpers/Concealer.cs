using SvenVissers.Steganography.Messages;
using System.Collections;
using System.Drawing;

namespace SvenVissers.Steganography.Helpers
{
    public class Concealer
    {
        private int maxPixels;
        private int imageWidth;

        private int pixelIndex = 0;
        private int totalBitsConcealed = 0;

        private Message message;

        private bool fullMessageConcealed = false;
        private byte numberOfConcealBits;


        public Bitmap Conceal(Bitmap image, Message message)
        {
            this.message = message;
            maxPixels = image.Width * image.Height;
            imageWidth = image.Width;
            numberOfConcealBits = message.CalculateNumberOfConcealBits(image.Width, image.Height);

            Bitmap concealedImage = new Bitmap(image);
            BitArray binaryMessage = new BitArray(message.GetBinaryMessage());

            concealedImage = ConcealFullMessage(concealedImage, binaryMessage);

            return concealedImage;
        }

        private Bitmap ConcealFullMessage(Bitmap concealedImage, BitArray binaryMessage)
        {
            while (!fullMessageConcealed && pixelIndex < maxPixels)
            {
                Color pixel = concealedImage.GetPixel(pixelIndex % imageWidth, pixelIndex / imageWidth);
                pixel = ConcealInPixel(pixel, binaryMessage);

                concealedImage.SetPixel(pixelIndex % imageWidth, pixelIndex / imageWidth, pixel);
                pixelIndex++;
            }

            return concealedImage;
        }

        private Color ConcealInPixel(Color pixel, BitArray binaryMessage)
        {
            for (int c = 0; c < 3; c++)
            {
                byte channel = PixelHelper.GetChannelFromColorByIndex(pixel, (ColorIndex)c);
                channel = ConcealInPixelChanelByte(channel, binaryMessage);

                pixel = PixelHelper.SetChannelFromColorByIndex(pixel, (ColorIndex)c, channel);

                if (fullMessageConcealed)
                {
                    break;
                }
            }

            return pixel;
        }

        private byte ConcealInPixelChanelByte(byte chanel, BitArray binaryMessage)
        {
            int byteIndex = totalBitsConcealed / 8;
            int numberOfConcealBits = (byteIndex < 2) ? 1 : this.numberOfConcealBits;

            for (int i = 0; i < numberOfConcealBits; i++)
            {
                if (totalBitsConcealed >= binaryMessage.Length)
                {
                    break;
                }

                ByteHelper.StoreBitValueInByte(ref chanel, i, binaryMessage.Get(totalBitsConcealed));
                totalBitsConcealed++;

                fullMessageConcealed = (totalBitsConcealed >= binaryMessage.Count);

                if (fullMessageConcealed)
                {
                    break;
                }
            }

            return chanel;
        }
    }
}
