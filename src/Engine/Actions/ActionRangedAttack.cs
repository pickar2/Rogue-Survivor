using RogueSurvivor.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RogueSurvivor.Engine.Actions
{
    class ActionRangedAttack : ActorAction
    {
        Actor m_Target;
        List<Point> m_LoF = new List<Point>();
        FireMode m_Mode;

        public ActionRangedAttack(Actor actor, RogueGame game, Actor target, FireMode mode)
            : base(actor, game)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            m_Target = target;
            m_Mode = mode;
        }

        public ActionRangedAttack(Actor actor, RogueGame game, Actor target)
            : this(actor, game, target, FireMode.DEFAULT)
        {
        }

        public override bool IsLegal()
        {
            m_LoF.Clear();
            return m_Game.Rules.CanActorFireAt(m_Actor, m_Target, m_LoF, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoRangedAttack(m_Actor, m_Target, m_LoF, m_Mode);
        }
    }
}
