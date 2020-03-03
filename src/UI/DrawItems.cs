using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueSurvivor.UI
{
    interface IDrawItem
    {
    }

    class DrawTextItem : IDrawItem
    {
        public Color color;
        public Color? shadowColor;
        public SpriteFont font;
        public string text;
        public Vector2 pos;
    }

    public class DrawLineItem : IDrawItem
    {
        public Color color;
        public Vector2 from, to;
    }

    public class DrawImageItem : IDrawItem
    {
        public Texture2D image;
        public Vector2 pos;
        public Color tint;
        public float rotation;
        public float scale;
        public bool transform;
    }

    public class DrawRectangleItem : IDrawItem
    {
        public Rectangle rectangle;
        public Color color;
        public bool filled;
    }
}
