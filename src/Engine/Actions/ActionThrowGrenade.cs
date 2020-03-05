using RogueSurvivor.Data;
using RogueSurvivor.Engine.Items;
using System.Drawing;

namespace RogueSurvivor.Engine.Actions
{
    class ActionThrowGrenade : ActorAction
    {
        Point m_ThrowPos;

        public ActionThrowGrenade(Actor actor, RogueGame game, Point throwPos)
            : base(actor, game)
        {
            m_ThrowPos = throwPos;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorThrowTo(m_Actor, m_ThrowPos, null, out m_FailReason);
        }

        public override void Perform()
        {
            Item grenade = m_Actor.GetEquippedWeapon();

            if (grenade is ItemPrimedExplosive)
                m_Game.DoThrowGrenadePrimed(m_Actor, m_ThrowPos);
            else
                m_Game.DoThrowGrenadeUnprimed(m_Actor, m_ThrowPos);
        }
    }
}
