using System;

namespace RogueSurvivor.Engine
{
    [Serializable]
    class DiceRoller
    {
        Random m_Rng;

        public DiceRoller(int seed)
        {
            m_Rng = new Random(seed);
        }

        /// <summary>
        /// Seed with current time.
        /// </summary>
        public DiceRoller()
            : this((int)DateTime.UtcNow.Ticks)
        {
        }

        /// <summary>
        /// Roll in range [min, max[.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int Roll(int min, int max)
        {
            // sanity check, fixes crashes.
            if (max <= min) return min;

            // roll it baby.
            int r;
            lock (m_Rng) // thread safe, Random is supposed to be thread safe but apparently not...
            {
                r = m_Rng.Next(min, max);
            }
            // FIX awfull bug, in some very rare cases .NET Random returns max instead of max-1 (wtf?!)
            if (r >= max) r = max - 1;

            return r;
        }

        public float RollFloat()
        {
            float r;
            lock (m_Rng) // thread safe, Random is supposed to be thread safe but apparently not...
            {
                r = (float)m_Rng.NextDouble();
            }
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chance">chance as a percentage [0..100]</param>
        /// <returns></returns>
        public bool RollChance(int chance)
        {
            return Roll(0, 100) < chance;
        }
    }
}
