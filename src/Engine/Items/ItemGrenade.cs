using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemGrenade : ItemExplosive
    {
        public ItemGrenade(ItemModel model, ItemModel primedModel)
            : base(model, primedModel)
        {
            if (!(model is ItemGrenadeModel))
                throw new ArgumentException("model is not ItemGrenadeModel");
        }
    }
}
