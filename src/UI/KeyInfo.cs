namespace RogueSurvivor.UI
{
    public class KeyInfo
    {
        private Key key;

        public KeyInfo(Key key)
        {
            this.key = key;
        }

        public Key Modifiers => key & Key.Modifiers;
        public Key KeyCode => key & ~Key.Modifiers;
        public bool Alt => (key & Key.Alt) != 0;
        public bool Control => (key & Key.Control) != 0;
        public bool Shift => (key & Key.Shift) != 0;
    }
}
