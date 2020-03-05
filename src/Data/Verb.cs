using System;

namespace RogueSurvivor.Data
{
    [Serializable]
    class Verb
    {
        public string YouForm { get; protected set; }
        public string HeForm { get; protected set; }

        public Verb(string youForm, string heForm)
        {
            this.YouForm = youForm;
            this.HeForm = heForm;
        }

        public Verb(string youForm) : this(youForm, youForm + "s")
        {
        }
    }
}
