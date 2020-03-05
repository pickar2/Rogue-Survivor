using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Actions
{
    class ActionRechargeItemBattery : ActorAction
    {
        Item m_Item;

        public ActionRechargeItemBattery(Actor actor, RogueGame game, Item it)
            : base(actor, game)
        {
            if (it == null)
                throw new ArgumentNullException("item");

            m_Item = it;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorRechargeItemBattery(m_Actor, m_Item, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoRechargeItemBattery(m_Actor, m_Item);
        }
    }
}
