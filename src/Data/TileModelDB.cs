namespace RogueSurvivor.Data
{
    abstract class TileModelDB 
    {
        public abstract TileModel this[int id]
        {
            get;
        }
    }
}
