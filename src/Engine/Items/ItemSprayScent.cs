using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemSprayScent : Item
    {
        public int SprayQuantity { get; set; }
        public Odor Odor { get { return (this.Model as ItemSprayScentModel).Odor; } }
        public int Strength { get { return (this.Model as ItemSprayScentModel).Strength; } }

        public ItemSprayScent(ItemModel model)
            : base(model)
        {
            if (!(model is ItemSprayScentModel))
                throw new ArgumentException("model is not a ItemScentSprayModel");

            this.SprayQuantity = (model as ItemSprayScentModel).MaxSprayQuantity;
        }
    }
}
