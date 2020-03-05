using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemTrap : Item
    {
        bool m_IsActivated;
        bool m_IsTriggered;
        Actor m_Owner;

        public bool IsActivated
        {
            get { return m_IsActivated; }
        }

        public bool IsTriggered
        {
            get { return m_IsTriggered; }
            set { m_IsTriggered = value; }
        }

        public ItemTrapModel TrapModel { get { return Model as ItemTrapModel; } }

        public Actor Owner
        {
            get
            {
                // cleanup dead owner reference
                if (m_Owner != null && m_Owner.IsDead)
                    m_Owner = null;

                return m_Owner;
            }
        }

        public ItemTrap(ItemModel model)
            : base(model)
        {
            if (!(model is ItemTrapModel))
                throw new ArgumentException("model is not a TrapModel");
        }

        /// <summary>
        /// A new trap of the same model, un-activated, no owner, un-triggered.
        /// </summary>
        /// <returns></returns>
        public ItemTrap Clone()
        {
            ItemTrap c = new ItemTrap(TrapModel);
            return c;
        }

        public void Activate(Actor owner)
        {
            m_Owner = owner;
            m_IsActivated = true;
        }

        public void Desactivate()
        {
            m_Owner = null;
            m_IsActivated = false;
        }

        public override void OptimizeBeforeSaving()
        {
            base.OptimizeBeforeSaving();

            // cleanup dead owner ref
            if (m_Owner != null && m_Owner.IsDead)
                m_Owner = null;
        }
    }
}
