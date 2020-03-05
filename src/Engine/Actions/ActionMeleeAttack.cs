using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Actions
{
    class ActionMeleeAttack : ActorAction
    {
        readonly Actor m_Target;

        public ActionMeleeAttack(Actor actor, RogueGame game, Actor target)
            : base(actor, game)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            m_Target = target;
        }

        public override bool IsLegal()
        {
            return true;    // handled before in rules
        }

        public override void Perform()
        {
            m_Game.DoMeleeAttack(m_Actor, m_Target);
        }
    }
}
