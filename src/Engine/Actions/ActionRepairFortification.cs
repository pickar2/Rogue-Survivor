using RogueSurvivor.Data;
using RogueSurvivor.Engine.MapObjects;
using System;

namespace RogueSurvivor.Engine.Actions
{
    class ActionRepairFortification : ActorAction
    {
        Fortification m_Fort;

        public ActionRepairFortification(Actor actor, RogueGame game, Fortification fort)
            : base(actor, game)
        {
            if (fort == null)
                throw new ArgumentNullException("fort");

            m_Fort = fort;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorRepairFortification(m_Actor, m_Fort, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoRepairFortification(m_Actor, m_Fort);
        }
    }
}
