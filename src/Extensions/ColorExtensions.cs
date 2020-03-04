using System.Drawing;
using Xna = Microsoft.Xna.Framework;

namespace RogueSurvivor.Extensions
{
    public static class ColorExtensions
    {
        public static float GetBrightness(this Xna.Color color)
        {
            return color.FromXna().GetBrightness();
        }

        public static Xna.Color ToXna(this Color color)
        {
            return new Xna.Color(color.R, color.G, color.B, color.A);
        }

        public static Xna.Color? ToXna(this Color? color)
        {
            if (color.HasValue)
                return ToXna(color.Value);
            return null;
        }

        public static Color FromXna(this Xna.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
