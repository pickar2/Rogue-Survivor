using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemAmmo : Item
    {
        AmmoType m_AmmoType;

        public AmmoType AmmoType
        {
            get { return m_AmmoType; }
        }

        public ItemAmmo(ItemModel model)
            : base(model)
        {
            if (!(model is ItemAmmoModel))
                throw new ArgumentException("model is not a AmmoModel");

            ItemAmmoModel m = model as ItemAmmoModel;
            m_AmmoType = m.AmmoType;
            this.Quantity = m.MaxQuantity;
        }
    }
}
