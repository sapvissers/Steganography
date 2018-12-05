using SvenVissers.Steganography.Exceptions;
using SvenVissers.Steganography.Helpers;
using SvenVissers.Steganography.Messages;
using System;
using System.Drawing;
using System.Text;
using Xunit;

namespace SteganographyTest.Helpers
{
    public class PixelHelperTest
    {
        [Fact]
        public void Should_get_correct_channels()
        {
            //arrange
            Random random = new Random();
            Color pixel = Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

            //act
            byte rChannel = PixelHelper.GetChannelFromColorByIndex(pixel, ColorIndex.R);
            byte gChannel = PixelHelper.GetChannelFromColorByIndex(pixel, ColorIndex.G);
            byte bChannel = PixelHelper.GetChannelFromColorByIndex(pixel, ColorIndex.B);

            //assert
            Assert.Equal(pixel.R, rChannel);
            Assert.Equal(pixel.G, gChannel);
            Assert.Equal(pixel.B, bChannel);
        }

        [Fact]
        public void Should_set_correct_channels()
        {
            //arrange
            Random random = new Random();
            Color pixel = Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

            //act
            pixel = PixelHelper.SetChannelFromColorByIndex(pixel, ColorIndex.R, 5);
            pixel = PixelHelper.SetChannelFromColorByIndex(pixel, ColorIndex.G, 10);
            pixel = PixelHelper.SetChannelFromColorByIndex(pixel, ColorIndex.B, 15);

            //assert
            Assert.Equal(5, pixel.R);
            Assert.Equal(10, pixel.G);
            Assert.Equal(15, pixel.B);
        }
    }
}
