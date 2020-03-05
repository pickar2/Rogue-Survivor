using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemGrenadePrimed : ItemPrimedExplosive
    {
        public ItemGrenadePrimed(ItemModel model)
            : base(model)
        {
            if (!(model is ItemGrenadePrimedModel))
                throw new ArgumentException("model is not ItemGrenadePrimedModel");
        }
    }
}
