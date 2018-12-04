using SvenVissers.Steganography.Helpers;
using Xunit;

namespace SteganographyTest.Helpers
{
    public class ByteHelperTest
    {
        [Fact]
        public void Should_set_first_byte_to_true()
        {
            //arrange
            byte @byte = 0x00; // 00000000

            //act
            ByteHelper.StoreBitValueInByte(ref @byte, 0, true);

            //assert
            Assert.Equal(0x01, @byte); // 00000001
        }

        [Fact]
        public void Should_set_second_byte_to_true()
        {
            //arrange
            byte @byte = 0x00; // 00000000

            //act
            ByteHelper.StoreBitValueInByte(ref @byte, 1, true);

            //assert
            Assert.Equal(0x02, @byte); // 00000010
        }

        [Fact]
        public void Should_set_first_byte_to_false()
        {
            //arrange
            byte @byte = 0x01; // 00000001

            //act
            ByteHelper.StoreBitValueInByte(ref @byte, 0, false);

            //assert
            Assert.Equal(0x00, @byte); // 00000000
        }

        [Fact]
        public void Should_set_second_byte_to_false()
        {
            //arrange
            byte @byte = 0x02; // 00000010

            //act
            ByteHelper.StoreBitValueInByte(ref @byte, 1, false);

            //assert
            Assert.Equal(0x00, @byte); // 00000000
        }

        [Fact]
        public void Should_read_byte_value_at_position()
        {
            //arrange
            byte @byte = 0x55; // 01010101

            //act
            bool value = ByteHelper.GetBitValueInByte(@byte, 2);

            //assert
            Assert.True(value);
        }
    }
}
