using SvenVissers.Steganography.Helpers;
using SvenVissers.Steganography.Messages;
using System.Drawing;

namespace SvenVissers.Steganography
{
    public class Generator
    {
        #region Conceal
        public static Bitmap Conceal(Bitmap image, Message message)
        {
            return Concealer.Conceal(image, message);
        }
        #endregion

        #region Reveal
        public static Message Reveal(Bitmap image)
        {
            return Revealer.Reveal(image);
        }
        #endregion
    }
}