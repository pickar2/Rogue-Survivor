using RogueSurvivor.Data;
using System;
using System.Collections.Generic;

namespace RogueSurvivor.Engine.AI
{
    [Serializable]
    abstract class Sensor
    {
        public abstract List<Percept> Sense(RogueGame game, Actor actor);
    }
}
