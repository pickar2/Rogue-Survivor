namespace RogueSurvivor.Data
{
    abstract class FactionDB
    {
        public abstract Faction this[int id]
        {
            get;
        }
    }
}
