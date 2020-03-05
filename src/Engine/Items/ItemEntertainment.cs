using RogueSurvivor.Data;
using System;
using System.Collections.Generic;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemEntertainment : Item
    {
        List<Actor> m_BoringFor = null;

        public ItemEntertainmentModel EntertainmentModel { get { return this.Model as ItemEntertainmentModel; } }

        public ItemEntertainment(ItemModel model)
            : base(model)
        {
            if (!(model is ItemEntertainmentModel))
                throw new ArgumentException("model is not a EntertainmentModel");
        }

        public void AddBoringFor(Actor a)
        {
            if (m_BoringFor == null) m_BoringFor = new List<Actor>(1);
            if (m_BoringFor.Contains(a)) return;
            m_BoringFor.Add(a);
        }

        public bool IsBoringFor(Actor a)
        {
            if (m_BoringFor == null) return false;
            return m_BoringFor.Contains(a);
        }

        public override void OptimizeBeforeSaving()
        {
            base.OptimizeBeforeSaving();

            // clean up dead actors refs
            // side effect: revived actors will forget about boring items
            if (m_BoringFor != null)
            {
                for (int i = 0; i < m_BoringFor.Count;)
                {
                    if (m_BoringFor[i].IsDead)
                        m_BoringFor.RemoveAt(i);
                    else
                        i++;
                }
                if (m_BoringFor.Count == 0)
                    m_BoringFor = null;
            }
        }
    }
}
