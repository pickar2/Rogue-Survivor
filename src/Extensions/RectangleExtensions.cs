using Microsoft.Xna.Framework;

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
    }
}
