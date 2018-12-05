using SvenVissers.Steganography;
using SvenVissers.Steganography.Messages;
using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
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
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
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
            Assert.True(revealedMessage.Success);
            Assert.Equal<MessageType>(MessageType.UTF8_TEXT, revealedMessage.MessageType);
            Assert.Equal(Convert.ToByte(1), revealedMessage.NumberOfConcealBits);
            Assert.Equal(messageBody, Encoding.UTF8.GetString(revealedMessage.BinaryMessageBody));
        }

        [Fact]
        public async Task Should_conceal_and_reveal_the_same_message_asynchronously()
        {
            //arrange
            Bitmap image = new Bitmap(100, 100);
            Random random = new Random();

            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    image.SetPixel(x, y, Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
                }
            }

            string messageBody = "This is a test message.";
            Message message = new Message(messageBody);

            //act
            Bitmap concealedImage = await Generator.ConcealAsync(image, message);
            Message revealedMessage = await Generator.RevealAsync(concealedImage);

            //assert
            Assert.True(revealedMessage.Success);
            Assert.Equal<MessageType>(MessageType.UTF8_TEXT, revealedMessage.MessageType);
            Assert.Equal(Convert.ToByte(1), revealedMessage.NumberOfConcealBits);
            Assert.Equal(messageBody, Encoding.UTF8.GetString(revealedMessage.BinaryMessageBody));
        }
    }
}
