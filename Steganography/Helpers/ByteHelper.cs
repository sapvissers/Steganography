namespace SvenVissers.Steganography.Helpers
{
    public class ByteHelper
    {
        public static void StoreBitValueInByte(ref byte @byte, int position, bool value)
        {
            @byte = (byte)((value) ? (@byte | (1 << position)) : (@byte & ~(1 << position)));
        }

        public static bool GetBitValueInByte(byte @byte, int position)
        {
            return (@byte & (1 << position)) != 0;
        }
    }
}
