using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Actions
{
    class ActionStartDragCorpse : ActorAction
    {
        readonly Corpse m_Target;

        public ActionStartDragCorpse(Actor actor, RogueGame game, Corpse target)
            : base(actor, game)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            m_Target = target;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorStartDragCorpse(m_Actor, m_Target, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoStartDragCorpse(m_Actor, m_Target);
        }
    }
}
