using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemPrimedExplosive : ItemExplosive
    {
        public int FuseTimeLeft { get; set; }

        public ItemPrimedExplosive(ItemModel model)
            : base(model, model)
        {
            if (!(model is ItemExplosiveModel))
                throw new ArgumentException("model is not ItemExplosiveModel");

            this.FuseTimeLeft = (model as ItemExplosiveModel).FuseDelay;
        }
    }
}
