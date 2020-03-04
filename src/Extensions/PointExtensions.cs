using System.Drawing;
using Xna = Microsoft.Xna.Framework;

namespace RogueSurvivor.Extensions
{
    public static class PointExtensions
    {
        public static Point Add(this Point pt, int x, int y)
        {
            return new Point(pt.X + x, pt.Y + y);
        }

        public static Point Add(this Point pt, Point other)
        {
            return new Point(pt.X + other.X, pt.Y + other.Y);
        }

        public static Point FromXna(this Xna.Point pt)
        {
            return new Point(pt.X, pt.Y);
        }
    }
}
