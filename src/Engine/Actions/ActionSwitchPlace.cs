using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Actions
{
    class ActionSwitchPlace : ActorAction
    {
        readonly Actor m_Target;

        public ActionSwitchPlace(Actor actor, RogueGame game, Actor target)
            : base(actor, game)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            m_Target = target;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorSwitchPlaceWith(m_Actor, m_Target);
        }

        public override void Perform()
        {
            m_Game.DoSwitchPlace(m_Actor, m_Target);
        }
    }
}
