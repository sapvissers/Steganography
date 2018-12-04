using SvenVissers.Steganography.Exceptions;
using SvenVissers.Steganography.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SteganographyTest.Messages
{
    public class MessageTest
    {
        [Fact]
        public void Should_construct_message_with_text_body()
        {
            //arrange
            string messageBody = "This is a test.";
            byte[] binaryMessage = Encoding.UTF8.GetBytes(messageBody);

            //act
            Message message = new Message(messageBody);

            //assert
            Assert.Equal(binaryMessage, message.BinaryMessageBody);
            Assert.Equal(MessageType.UTF8_TEXT, message.MessageType);
        }

        [Fact]
        public void Should_construct_message_with_binary_body()
        {
            //arrange
            Random random = new Random();
            byte[] binaryMessage = new byte[10];
            random.NextBytes(binaryMessage);

            //act
            Message message = new Message(binaryMessage, MessageType.BINARY);

            //assert
            Assert.Equal(binaryMessage, message.BinaryMessageBody);
            Assert.Equal(MessageType.BINARY, message.MessageType);
        }

        [Fact]
        public void Should_construct_message_with_binary_message_array()
        {
            //arrange
            byte prefix = 0x0f;
            byte numberOfConcealBits = 0x01;
            byte messageType = Convert.ToByte(MessageType.BINARY);
            byte suffix = 0x00;

            Random random = new Random();
            byte[] binaryMessage = new byte[10];
            random.NextBytes(binaryMessage);

            List<byte> byteList = new List<byte>();
            byteList.Add(prefix);
            byteList.Add(numberOfConcealBits);
            byteList.Add(messageType);
            byteList.AddRange(binaryMessage);
            byteList.Add(suffix);

            //act
            Message message = new Message(byteList.ToArray());

            //assert
            Assert.Equal(numberOfConcealBits, message.NumberOfConcealBits);
            Assert.Equal(MessageType.BINARY, message.MessageType);
            Assert.True(message.Success);
            Assert.Equal(binaryMessage, message.BinaryMessageBody);
        }

        [Fact]
        public void Should_calculate_one_number_of_conceal_bit()
        {
            //arrange
            Message message = new Message("Test");

            //act
            message.CalculateNumberOfConcealBits(5, 5);

            //assert
            Assert.Equal(0x01, message.NumberOfConcealBits);
        }

        [Fact]
        public void Should_calculate_four_number_of_conceal_bit()
        {
            //arrange
            Message message = new Message("This is a bigger message.");

            //act
            message.CalculateNumberOfConcealBits(5, 5);

            //assert
            Assert.Equal(0x04, message.NumberOfConcealBits);
        }

        [Fact]
        public void Should_throw_message_to_big_exception()
        {
            //arrange
            Message message = new Message("This is a very big message placed inside a very small image");

            //act
            Action act = () => message.CalculateNumberOfConcealBits(3, 3);

            //assert
            Assert.Throws<MessageToBigException>(act);
        }

        [Fact]
        public void Should_arrange_a_correct_binary_message()
        {
            //arrange
            byte prefix = 0x0f;
            byte numberOfConcealBits = 0x01;
            byte messageType = Convert.ToByte(MessageType.BINARY);
            byte suffix = 0x00;

            Random random = new Random();
            byte[] binaryMessage = new byte[10];
            random.NextBytes(binaryMessage);

            List<byte> byteList = new List<byte>();
            byteList.Add(prefix);
            byteList.Add(numberOfConcealBits);
            byteList.Add(messageType);
            byteList.AddRange(binaryMessage);
            byteList.Add(suffix);

            Message message = new Message(binaryMessage, MessageType.BINARY);
            message.CalculateNumberOfConcealBits(100, 100);

            //act
            byte[] generatedBinaryMessage = message.GetBinaryMessage();

            //assert
            Assert.Equal(byteList.ToArray(), generatedBinaryMessage);
        }
    }
}
