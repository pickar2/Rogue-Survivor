using System.Drawing;
using Xna = Microsoft.Xna.Framework;

namespace RogueSurvivor.Extensions
{
    public static class RectangleExtensions
    {
        public static Rectangle Create(int left, int top, int right, int bottom)
        {
            return new Rectangle(left,
                                 top,
                                 right - left,
                                 bottom - top);
        }

        public static Xna.Rectangle ToXna(this Rectangle rect)
        {
            return new Xna.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
