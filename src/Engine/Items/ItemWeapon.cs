using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemWeapon : Item
    {
        public ItemWeapon(ItemModel model)
            : base(model)
        {
            if (!(model is ItemWeaponModel))
                throw new ArgumentException("model is not a WeaponModel");
        }
    }
}
