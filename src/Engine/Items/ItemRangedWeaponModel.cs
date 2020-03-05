using RogueSurvivor.Data;

namespace RogueSurvivor.Engine.Items
{
    class ItemRangedWeaponModel : ItemWeaponModel
    {
        int m_MaxAmmo;
        AmmoType m_AmmoType;

        public bool IsFireArm
        {
            get { return this.Attack.Kind == AttackKind.FIREARM; }
        }

        public bool IsBow
        {
            get { return this.Attack.Kind == AttackKind.BOW; }
        }

        public int MaxAmmo
        {
            get { return m_MaxAmmo; }
        }

        public AmmoType AmmoType
        {
            get { return m_AmmoType; }
        }

        public int RapidFireHit1Value
        {
            get { return Attack.Hit2Value; }
        }

        public int RapidFireHit2Value
        {
            get { return Attack.Hit3Value; }
        }

        public ItemRangedWeaponModel(string aName, string theNames, string imageID, Attack attack, int maxAmmo, AmmoType ammoType)
            : base(aName, theNames, imageID, attack)
        {
            m_MaxAmmo = maxAmmo;
            m_AmmoType = ammoType;
        }
    }
}
