using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Items
{
    [Serializable]
    class ItemBarricadeMaterialModel : ItemModel
    {
        int m_BarricadingValue;

        public int BarricadingValue
        {
            get { return m_BarricadingValue; }
        }

        public ItemBarricadeMaterialModel(string aName, string theNames, string imageID, int barricadingValue)
            : base(aName, theNames, imageID)
        {
            m_BarricadingValue = barricadingValue;
        }
    }
}
