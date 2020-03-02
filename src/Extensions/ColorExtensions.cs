using Microsoft.Xna.Framework;

namespace RogueSurvivor.Extensions
{
    public static class ColorExtensions
    {
        public static float GetBrightness(this Color color)
        {
            float r = (float)color.R / 255.0f;
            float g = (float)color.G / 255.0f;
            float b = (float)color.B / 255.0f;

            float max, min;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            return (max + min) / 2;
        }
    }
}
