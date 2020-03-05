using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.AI
{
    [Serializable]
    class Percept
    {
        int m_Turn;
        Location m_Location;
        Object m_Percepted;

        public int Turn
        {
            get { return m_Turn; }
            set { m_Turn = value; }
        }

        public Object Percepted
        {
            get { return m_Percepted; }
        }

        public Location Location
        {
            get { return m_Location; }
            set { m_Location = value; }
        }

        public Percept(Object percepted, int turn, Location location)
        {
            if (percepted == null)
                throw new ArgumentNullException("percepted");

            m_Percepted = percepted;
            m_Turn = turn;
            m_Location = location;
        }

        public int GetAge(int currentGameTurn)
        {
            return Math.Max(0, currentGameTurn - m_Turn);
        }
    }
}
