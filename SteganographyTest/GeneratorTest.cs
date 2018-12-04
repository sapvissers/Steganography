using SvenVissers.Steganography;
using SvenVissers.Steganography.Messages;
using System;
using System.Drawing;
using System.Text;
using Xunit;

namespace SteganographyTest
{
    public class GeneratorTest
    {
        [Fact]
        public void Should_conceal_and_reveal_the_same_message()
        {
            //arrange
            Bitmap image = new Bitmap(100, 100);
            Random random = new Random();
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    image.SetPixel(x, y, Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
                }
            }

            string messageBody = "This is a test message.";
            Message message = new Message(messageBody);

            //act
            Bitmap concealedImage = Generator.Conceal(image, message);
            Message revealedMessage = Generator.Reveal(concealedImage);

            //assert
            Assert.True(message.Success);
            Assert.Equal<MessageType>(MessageType.UTF8_TEXT, message.MessageType);
            Assert.Equal(Convert.ToByte(1), message.NumberOfConcealBits);
            Assert.Equal(messageBody, Encoding.UTF8.GetString(message.BinaryMessageBody));
        }
    }
}
