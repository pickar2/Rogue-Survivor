using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemBarricadeMaterial : Item
    {
        public ItemBarricadeMaterial(ItemModel model) : base(model)
        {
            if (!(model is ItemBarricadeMaterialModel))
                throw new ArgumentException("model is not BarricadeMaterialModel");
        }
    }
}
