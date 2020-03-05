using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemExplosive : Item
    {
        public int PrimedModelID { get; private set; }

        public ItemExplosive(ItemModel model, ItemModel primedModel)
            : base(model)
        {
            if (!(model is ItemExplosiveModel))
                throw new ArgumentException("model is not ItemExplosiveModel");

            this.PrimedModelID = primedModel.ID;
        }
    }
}
