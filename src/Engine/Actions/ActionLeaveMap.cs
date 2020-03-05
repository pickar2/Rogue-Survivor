using RogueSurvivor.Data;
using System.Drawing;

namespace RogueSurvivor.Engine.Actions
{
    class ActionLeaveMap : ActorAction
    {
        Point m_ExitPoint;

        public Point ExitPoint
        {
            get { return m_ExitPoint; }
        }

        public ActionLeaveMap(Actor actor, RogueGame game, Point exitPoint)
            : base(actor, game)
        {
            m_ExitPoint = exitPoint;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorLeaveMap(m_Actor, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoLeaveMap(m_Actor, m_ExitPoint, true);
        }
    }
}
