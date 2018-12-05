using System;
using System.Drawing;

namespace SvenVissers.Steganography.Helpers
{
    public static class PixelHelper
    {
        public static byte GetChannelFromColorByIndex(Color pixel, ColorIndex index)
        {
            switch (index)
            {
                case ColorIndex.A:
                    return pixel.A;
                case ColorIndex.R:
                    return pixel.R;
                case ColorIndex.G:
                    return pixel.G;
                case ColorIndex.B:
                    return pixel.B;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public static Color SetChannelFromColorByIndex(Color pixel, ColorIndex index, byte value)
        {
            byte A = pixel.A;
            byte R = pixel.R;
            byte G = pixel.G;
            byte B = pixel.B;

            switch (index)
            {
                case ColorIndex.A:
                    A = value;
                    break;
                case ColorIndex.R:
                    R = value;
                    break;
                case ColorIndex.G:
                    G = value;
                    break;
                case ColorIndex.B:
                    B = value;
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }

            return Color.FromArgb(A, R, G, B);
        }
    }
}
