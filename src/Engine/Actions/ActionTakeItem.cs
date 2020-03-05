using RogueSurvivor.Data;
using System;
using System.Drawing;

namespace RogueSurvivor.Engine.Actions
{
    class ActionTakeItem : ActorAction
    {
        Point m_Position;
        Item m_Item;

        public ActionTakeItem(Actor actor, RogueGame game, Point position, Item it)
            : base(actor, game)
        {
            if (it == null)
                throw new ArgumentNullException("item");

            m_Position = position;
            m_Item = it;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorGetItem(m_Actor, m_Item, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoTakeItem(m_Actor, m_Position, m_Item);
        }
    }
}