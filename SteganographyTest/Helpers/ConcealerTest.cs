using SvenVissers.Steganography.Exceptions;
using SvenVissers.Steganography.Helpers;
using SvenVissers.Steganography.Messages;
using System;
using System.Drawing;
using Xunit;

namespace SteganographyTest.Helpers
{
    public class ConcealerTest
    {
        [Fact]
        public void Should_throw_message_to_big_exception_if_image_is_to_small()
        {
            //arrange
            Message message = new Message("This is a very big message placed inside a very small image");
            Bitmap testImage = new Bitmap(3, 3);

            //act
            Action act = () => Concealer.Conceal(testImage, message);

            //assert
            Assert.Throws<MessageToBigException>(act);
        }

        [Fact]
        public void Should_successfully_conceal_message_inside_image_with_single_layer_of_bit_depth()
        {
            //arrange
            Message message = new Message("Test");

            Bitmap testImage = new Bitmap(5, 5);
            for (int y = 0; y < testImage.Height; y++)
            {
                for (int x = 0; x < testImage.Width; x++)
                {
                    testImage.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                }
            }

            Color[,] assertPixels = new Color[5, 5];
            assertPixels[0, 0] = Color.FromArgb(255, 1, 1, 1);
            assertPixels[0, 1] = Color.FromArgb(255, 1, 0, 0);
            assertPixels[0, 2] = Color.FromArgb(255, 0, 0, 1);
            assertPixels[0, 3] = Color.FromArgb(255, 0, 0, 0);
            assertPixels[0, 4] = Color.FromArgb(255, 0, 0, 0);

            assertPixels[1, 0] = Color.FromArgb(255, 0, 0, 0);
            assertPixels[1, 1] = Color.FromArgb(255, 1, 0, 0);
            assertPixels[1, 2] = Color.FromArgb(255, 0, 0, 0);
            assertPixels[1, 3] = Color.FromArgb(255, 0, 0, 1);
            assertPixels[1, 4] = Color.FromArgb(255, 0, 1, 0);

            assertPixels[2, 0] = Color.FromArgb(255, 1, 0, 1);
            assertPixels[2, 1] = Color.FromArgb(255, 0, 1, 0);
            assertPixels[2, 2] = Color.FromArgb(255, 0, 1, 1);
            assertPixels[2, 3] = Color.FromArgb(255, 0, 1, 1);
            assertPixels[2, 4] = Color.FromArgb(255, 0, 0, 1);

            assertPixels[3, 0] = Color.FromArgb(255, 1, 1, 0);
            assertPixels[3, 1] = Color.FromArgb(255, 0, 0, 1);
            assertPixels[3, 2] = Color.FromArgb(255, 0, 1, 1);
            assertPixels[3, 3] = Color.FromArgb(255, 1, 0, 0);
            assertPixels[3, 4] = Color.FromArgb(255, 0, 0, 0);

            assertPixels[4, 0] = Color.FromArgb(255, 0, 0, 0);
            assertPixels[4, 1] = Color.FromArgb(255, 0, 0, 0);
            assertPixels[4, 2] = Color.FromArgb(255, 0, 0, 0);
            assertPixels[4, 3] = Color.FromArgb(255, 0, 0, 0);
            assertPixels[4, 4] = Color.FromArgb(255, 0, 0, 0);

            //act
            Bitmap resultImage = Concealer.Conceal(testImage, message);

            //assert
            for (int y = 0; y < resultImage.Height; y++)
            {
                for (int x = 0; x < resultImage.Width; x++)
                {
                    Color pixel = resultImage.GetPixel(x, y);
                    Assert.Equal(pixel, assertPixels[y, x]);
                }
            }
        }

        [Fact]
        public void Should_successfully_conceal_message_inside_image_with_multiple_layers_of_bit_depth()
        {
            //arrange
            Message message = new Message("This is a bigger message.");

            Bitmap testImage = new Bitmap(4, 4);
            for (int y = 0; y < testImage.Height; y++)
            {
                for (int x = 0; x < testImage.Width; x++)
                {
                    testImage.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                }
            }

            Color[,] assertPixels = new Color[5, 5];
            assertPixels[0, 0] = Color.FromArgb(255, 1, 1, 1);
            assertPixels[0, 1] = Color.FromArgb(255, 1, 0, 0);
            assertPixels[0, 2] = Color.FromArgb(255, 0, 0, 1);
            assertPixels[0, 3] = Color.FromArgb(255, 1, 1, 0);

            assertPixels[1, 0] = Color.FromArgb(255, 0, 0, 0);
            assertPixels[1, 1] = Color.FromArgb(255, 0, 4, 40);
            assertPixels[1, 2] = Color.FromArgb(255, 33, 75, 54);
            assertPixels[1, 3] = Color.FromArgb(255, 14, 72, 52);

            assertPixels[2, 0] = Color.FromArgb(255, 115, 64, 4);
            assertPixels[2, 1] = Color.FromArgb(255, 3, 34, 44);
            assertPixels[2, 2] = Color.FromArgb(255, 90, 51, 103);
            assertPixels[2, 3] = Color.FromArgb(255, 74, 73, 3);

            assertPixels[3, 0] = Color.FromArgb(255, 82, 45, 89);
            assertPixels[3, 1] = Color.FromArgb(255, 57, 115, 66);
            assertPixels[3, 2] = Color.FromArgb(255, 29, 43, 102);
            assertPixels[3, 3] = Color.FromArgb(255, 5, 0, 0);

            //act
            Bitmap resultImage = Concealer.Conceal(testImage, message);

            //assert
            for (int y = 0; y < resultImage.Height; y++)
            {
                for (int x = 0; x < resultImage.Width; x++)
                {
                    Color pixel = resultImage.GetPixel(x, y);
                    Assert.Equal(pixel, assertPixels[y, x]);
                }
            }
        }
    }
}
