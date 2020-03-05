using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemMeleeWeapon : ItemWeapon
    {
        public bool IsFragile
        {
            get { return (this.Model as ItemMeleeWeaponModel).IsFragile; }
        }

        public int ToolBashDamageBonus
        {
            get { return (this.Model as ItemMeleeWeaponModel).ToolBashDamageBonus; }
        }

        public float ToolBuildBonus
        {
            get { return (this.Model as ItemMeleeWeaponModel).ToolBuildBonus; }
        }

        public bool IsTool
        {
            get { return (this.Model as ItemMeleeWeaponModel).IsTool; }
        }

        public ItemMeleeWeapon(ItemModel model)
            : base(model)
        {
            if (!(model is ItemMeleeWeaponModel))
                throw new ArgumentException("model is not a MeleeWeaponModel");
        }
    }
}
