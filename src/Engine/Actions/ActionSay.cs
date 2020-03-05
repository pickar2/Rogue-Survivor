using RogueSurvivor.Data;
using System;

namespace RogueSurvivor.Engine.Actions
{
    class ActionSay : ActorAction
    {
        Actor m_Target;
        string m_Text;
        RogueGame.Sayflags m_Flags;

        public ActionSay(Actor actor, RogueGame game, Actor target, string text, RogueGame.Sayflags flags)
            : base(actor, game)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            m_Target = target;
            m_Text = text;
            m_Flags = flags;
        }

        public override bool IsLegal()
        {
            return true;
        }

        public override void Perform()
        {
            m_Game.DoSay(m_Actor, m_Target, m_Text, m_Flags);
        }
    }
}
