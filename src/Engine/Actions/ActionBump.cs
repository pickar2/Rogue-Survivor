
using RogueSurvivor.Data;

namespace RogueSurvivor.Engine.Actions
{
    /// <summary>
    /// High-level bump action that translate into a concrete action (move, use, attack, chat...). Very usefull for "intelligent" AI.
    /// </summary>
    class ActionBump : ActorAction
    {
        readonly Direction m_Direction;
        readonly Location m_NewLocation;
        readonly ActorAction m_ConcreteAction;

        public Direction Direction { get { return m_Direction; } }
        public ActorAction ConcreteAction { get { return m_ConcreteAction; } }

        public ActionBump(Actor actor, RogueGame game, Direction direction)
            : base(actor, game)
        {
            m_Direction = direction;
            m_NewLocation = actor.Location + direction;

            m_ConcreteAction = game.Rules.IsBumpableFor(m_Actor, game, m_NewLocation, out m_FailReason);
        }

        public override bool IsLegal()
        {
            if (m_ConcreteAction == null)
                return false;
            else
                return m_ConcreteAction.IsLegal();
        }

        public override void Perform()
        {
            if (m_ConcreteAction == null)
                return;
            else
                m_ConcreteAction.Perform();
        }
    }
}
