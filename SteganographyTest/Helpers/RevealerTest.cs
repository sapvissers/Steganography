using SvenVissers.Steganography.Exceptions;
using SvenVissers.Steganography.Helpers;
using SvenVissers.Steganography.Messages;
using System;
using System.Drawing;
using System.Text;
using Xunit;

namespace SteganographyTest.Helpers
{
    public class RevealerTest
    {
        [Fact]
        public void Should_throw_message_not_found_exception_if_no_message_is_present()
        {
            //arrange
            Bitmap image = new Bitmap(5, 5);
            Random random = new Random();
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        image.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                    }
                    else
                    {
                        image.SetPixel(x, y, Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
                    }

                }
            }

            //act
            Action act = () => Revealer.Reveal(image);

            //assert
            Assert.Throws<MessageNotFoundException>(act);
        }

        [Fact]
        public void Should_successfully_reveal_message_from_image_with_single_layer_of_bit_depth()
        {
            //arrange
            Bitmap testImage = new Bitmap(5, 5);
            testImage.SetPixel(0, 0, Color.FromArgb(255, 1, 1, 1));
            testImage.SetPixel(1, 0, Color.FromArgb(255, 1, 0, 0));
            testImage.SetPixel(2, 0, Color.FromArgb(255, 0, 0, 1));
            testImage.SetPixel(3, 0, Color.FromArgb(255, 0, 0, 0));
            testImage.SetPixel(4, 0, Color.FromArgb(255, 0, 0, 0));

            testImage.SetPixel(0, 1, Color.FromArgb(255, 0, 0, 0));
            testImage.SetPixel(1, 1, Color.FromArgb(255, 1, 0, 0));
            testImage.SetPixel(2, 1, Color.FromArgb(255, 0, 0, 0));
            testImage.SetPixel(3, 1, Color.FromArgb(255, 0, 0, 1));
            testImage.SetPixel(4, 1, Color.FromArgb(255, 0, 1, 0));

            testImage.SetPixel(0, 2, Color.FromArgb(255, 1, 0, 1));
            testImage.SetPixel(1, 2, Color.FromArgb(255, 0, 1, 0));
            testImage.SetPixel(2, 2, Color.FromArgb(255, 0, 1, 1));
            testImage.SetPixel(3, 2, Color.FromArgb(255, 0, 1, 1));
            testImage.SetPixel(4, 2, Color.FromArgb(255, 0, 0, 1));

            testImage.SetPixel(0, 3, Color.FromArgb(255, 1, 1, 0));
            testImage.SetPixel(1, 3, Color.FromArgb(255, 0, 0, 1));
            testImage.SetPixel(2, 3, Color.FromArgb(255, 0, 1, 1));
            testImage.SetPixel(3, 3, Color.FromArgb(255, 1, 0, 0));
            testImage.SetPixel(4, 3, Color.FromArgb(255, 0, 0, 0));

            testImage.SetPixel(0, 4, Color.FromArgb(255, 0, 0, 0));
            testImage.SetPixel(1, 4, Color.FromArgb(255, 0, 0, 0));
            testImage.SetPixel(2, 4, Color.FromArgb(255, 0, 0, 0));
            testImage.SetPixel(3, 4, Color.FromArgb(255, 0, 0, 0));
            testImage.SetPixel(4, 4, Color.FromArgb(255, 0, 0, 0));

            //act
            Message message = Revealer.Reveal(testImage);

            //assert
            Assert.True(message.Success);
            Assert.Equal<MessageType>(MessageType.UTF8_TEXT, message.MessageType);
            Assert.Equal(Convert.ToByte(1), message.NumberOfConcealBits);
            Assert.Equal("Test", Encoding.UTF8.GetString(message.BinaryMessageBody));
        }

        [Fact]
        public void Should_successfully_reveal_message_from_image_with_multiple_layers_of_bit_depth()
        {
            //arrange
            Bitmap testImage = new Bitmap(4, 4);

            testImage.SetPixel(0, 0, Color.FromArgb(255, 1, 1, 1));
            testImage.SetPixel(1, 0, Color.FromArgb(255, 1, 0, 0));
            testImage.SetPixel(2, 0, Color.FromArgb(255, 0, 0, 1));
            testImage.SetPixel(3, 0, Color.FromArgb(255, 1, 1, 0));

            testImage.SetPixel(0, 1, Color.FromArgb(255, 0, 0, 0));
            testImage.SetPixel(1, 1, Color.FromArgb(255, 0, 4, 40));
            testImage.SetPixel(2, 1, Color.FromArgb(255, 33, 75, 54));
            testImage.SetPixel(3, 1, Color.FromArgb(255, 14, 72, 52));

            testImage.SetPixel(0, 2, Color.FromArgb(255, 115, 64, 4));
            testImage.SetPixel(1, 2, Color.FromArgb(255, 3, 34, 44));
            testImage.SetPixel(2, 2, Color.FromArgb(255, 90, 51, 103));
            testImage.SetPixel(3, 2, Color.FromArgb(255, 74, 73, 3));

            testImage.SetPixel(0, 3, Color.FromArgb(255, 82, 45, 89));
            testImage.SetPixel(1, 3, Color.FromArgb(255, 57, 115, 66));
            testImage.SetPixel(2, 3, Color.FromArgb(255, 29, 43, 102));
            testImage.SetPixel(3, 3, Color.FromArgb(255, 5, 0, 0));

            //act
            Message message = Revealer.Reveal(testImage);

            //assert
            Assert.True(message.Success);
            Assert.Equal<MessageType>(MessageType.UTF8_TEXT, message.MessageType);
            Assert.Equal(Convert.ToByte(7), message.NumberOfConcealBits);
            Assert.Equal("This is a bigger message.", Encoding.UTF8.GetString(message.BinaryMessageBody));
        }
    }
}
