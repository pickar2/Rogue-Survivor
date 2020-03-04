using djack.RogueSurvivor.Data;
using System.Drawing;

namespace djack.RogueSurvivor.Engine.Actions
{
    class ActionBuildFortification : ActorAction
    {
        Point m_BuildPos;
        bool m_IsLarge;

        public ActionBuildFortification(Actor actor, RogueGame game, Point buildPos, bool isLarge)
            : base(actor, game)
        {
            m_BuildPos = buildPos;
            m_IsLarge = isLarge;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorBuildFortification(m_Actor, m_BuildPos, m_IsLarge);
        }

        public override void Perform()
        {
            m_Game.DoBuildFortification(m_Actor, m_BuildPos, m_IsLarge);
        }
    }
}
