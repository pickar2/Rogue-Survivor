using RogueSurvivor.Data;
using System.Drawing;

namespace RogueSurvivor.Engine.Actions
{
    class ActionMoveStep : ActorAction
    {
        Location m_NewLocation;

        public ActionMoveStep(Actor actor, RogueGame game, Direction direction)
            : base(actor, game)
        {
            m_NewLocation = actor.Location + direction;
        }

        public ActionMoveStep(Actor actor, RogueGame game, Point to)
            : base(actor, game)
        {
            m_NewLocation = new Location(actor.Location.Map, to);
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.IsWalkableFor(m_Actor, m_NewLocation, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoMoveActor(m_Actor, m_NewLocation);
        }
    }
}
