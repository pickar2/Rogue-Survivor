using RogueSurvivor.Data;

namespace RogueSurvivor.Engine.Actions
{
    class ActionWait : ActorAction
    {
        public ActionWait(Actor actor, RogueGame game)
            : base(actor, game)
        {
        }

        public override bool IsLegal()
        {
            return true;
        }

        public override void Perform()
        {
            m_Game.DoWait(m_Actor);
        }
    }
}
