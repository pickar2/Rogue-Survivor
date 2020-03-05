using RogueSurvivor.Data;
using System.Drawing;

namespace RogueSurvivor.Engine.Actions
{
    class ActionGetFromContainer : ActorAction
    {
        Point m_Position;

        /// <summary>
        /// Gets item that will be taken : top item from container position.
        /// </summary>
        public Item Item
        {
            get
            {
                Map map = m_Actor.Location.Map;
                return map.GetItemsAt(m_Position).TopItem;
            }
        }

        public ActionGetFromContainer(Actor actor, RogueGame game, Point position)
            : base(actor, game)
        {
            m_Position = position;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorGetItemFromContainer(m_Actor, m_Position, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoTakeFromContainer(m_Actor, m_Position);
        }
    }
}
