using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.MapObjects
{
    [Serializable]
    class Board : MapObject
    {
        public string[] Text
        {
            get;
            set;
        }

        public Board(string name, string imageID, string[] text)
            : base(name, imageID)
        {
            this.Text = text;
        }
    }
}
