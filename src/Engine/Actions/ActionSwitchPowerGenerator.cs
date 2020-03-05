using RogueSurvivor.Data;
using RogueSurvivor.Engine.MapObjects;
using System;

namespace RogueSurvivor.Engine.Actions
{
    class ActionSwitchPowerGenerator : ActorAction
    {
        PowerGenerator m_PowGen;

        public ActionSwitchPowerGenerator(Actor actor, RogueGame game, PowerGenerator powGen)
            : base(actor, game)
        {
            if (powGen == null)
                throw new ArgumentNullException("powGen");

            m_PowGen = powGen;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.IsSwitchableFor(m_Actor, m_PowGen, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoSwitchPowerGenerator(m_Actor, m_PowGen);
        }
    }
}
