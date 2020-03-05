using RogueSurvivor.Data;

namespace RogueSurvivor.Engine.Actions
{
    class ActionSleep : ActorAction
    {
        public ActionSleep(Actor actor, RogueGame game)
            : base(actor, game)
        {
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorSleep(m_Actor, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoStartSleeping(m_Actor);
        }
    }
}
