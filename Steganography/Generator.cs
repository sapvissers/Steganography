using SvenVissers.Steganography.Helpers;
using SvenVissers.Steganography.Messages;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace SvenVissers.Steganography
{
    public class Generator
    {
        #region Conceal
        public static Bitmap Conceal(Bitmap image, Message message)
        {
            Concealer concealer = new Concealer();

            return concealer.Conceal(image, message);
        }

        public static async Task<Bitmap> ConcealAsync(Bitmap image, Message message)
        {
            Concealer concealer = new Concealer();

            return await Task.Run(() => concealer.Conceal(image, message)); ;
        }
        #endregion

        #region Reveal
        public static Message Reveal(Bitmap image)
        {
            Revealer revealer = new Revealer();

            return revealer.Reveal(image);
        }

        public static async Task<Message> RevealAsync(Bitmap image)
        {
            Revealer revealer = new Revealer();

            return await Task.Run(() => revealer.Reveal(image));
        }
        #endregion
    }
}