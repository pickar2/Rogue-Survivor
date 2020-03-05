using RogueSurvivor.Data;
using RogueSurvivor.Engine;
using RogueSurvivor.Engine.AI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RogueSurvivor.Gameplay.AI.Sensors
{
    [Serializable]
    class SmellSensor : Sensor
    {
        [Serializable]
        public class AIScent
        {
            public Odor Odor { get; private set; }
            public int Strength { get; private set; }

            public AIScent(Odor odor, int strength)
            {
                this.Odor = odor;
                this.Strength = strength;
            }
        }

        Odor m_OdorToSmell;
        List<Percept> m_List;

        public List<Percept> Scents
        {
            get { return m_List; }
        }

        public SmellSensor(Odor odorToSmell)
        {
            m_OdorToSmell = odorToSmell;
            m_List = new List<Percept>(9);
        }

        public override List<Percept> Sense(RogueGame game, Actor actor)
        {
            m_List.Clear();
            int minStrength = game.Rules.ActorSmellThreshold(actor);

            // smell adjacent & self.
            int xmin = actor.Location.Position.X - 1;
            int xmax = actor.Location.Position.X + 1;
            int ymin = actor.Location.Position.Y - 1;
            int ymax = actor.Location.Position.Y + 1;
            actor.Location.Map.TrimToBounds(ref xmin, ref ymin);
            actor.Location.Map.TrimToBounds(ref xmax, ref ymax);

            int turnCounter = actor.Location.Map.LocalTime.TurnCounter;
            Point pt = new Point();
            for (int x = xmin; x <= xmax; x++)
            {
                pt.X = x;
                for (int y = ymin; y <= ymax; y++)
                {
                    pt.Y = y;
                    int strength = actor.Location.Map.GetScentByOdorAt(m_OdorToSmell, pt);
                    if (strength >= 0 && strength >= minStrength)
                        m_List.Add(new Percept(new AIScent(m_OdorToSmell, strength), turnCounter, new Location(actor.Location.Map, pt)));
                }
            }

            return m_List;
        }
    }
}
