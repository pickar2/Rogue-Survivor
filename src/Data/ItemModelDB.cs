namespace RogueSurvivor.Data
{
    abstract class ItemModelDB
    {
        public abstract ItemModel this[int id]
        {
            get;
        }
    }
}
