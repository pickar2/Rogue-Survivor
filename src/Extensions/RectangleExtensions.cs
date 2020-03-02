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

        public static void Intersect(this Rectangle rect, Rectangle rect2)
        {
            Rectangle result = Rectangle.Intersect(rect2, rect);

            rect.X = result.X;
            rect.Y = result.Y;
            rect.Width = result.Width;
            rect.Height = result.Height;
        }
    }
}
