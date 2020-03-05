namespace RogueSurvivor.Data
{
    abstract class ActorModelDB
    {
        public abstract ActorModel this[int id]
        {
            get;
        }
    }
}
