using RogueSurvivor.Data;
using System.Drawing;

namespace RogueSurvivor.Engine.Actions
{
    class ActionUseExit : ActorAction
    {
        Point m_ExitPoint;

        public ActionUseExit(Actor actor, Point exitPoint, RogueGame game)
            : base(actor, game)
        {
            m_ExitPoint = exitPoint;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorUseExit(m_Actor, m_ExitPoint, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoUseExit(m_Actor, m_ExitPoint);
        }
    }
}
