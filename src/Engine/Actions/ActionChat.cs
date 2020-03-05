using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Actions
{
    class ActionChat : ActorAction
    {
        readonly Actor m_Target;

        public Actor Target
        {
            get { return m_Target; }
        }

        public ActionChat(Actor actor, RogueGame game, Actor target)
            : base(actor, game)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            m_Target = target;
        }

        public override bool IsLegal()
        {
            return true;
        }

        public override void Perform()
        {
            m_Game.DoChat(m_Actor, m_Target);
        }
    }
}
