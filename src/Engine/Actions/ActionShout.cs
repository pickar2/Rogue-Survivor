using RogueSurvivor.Data;

namespace RogueSurvivor.Engine.Actions
{
    class ActionShout : ActorAction
    {
        string m_Text;

        public ActionShout(Actor actor, RogueGame game)
            : this(actor, game, null)
        {
        }

        public ActionShout(Actor actor, RogueGame game, string text)
            : base(actor, game)
        {
            m_Text = text;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorShout(m_Actor, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoShout(m_Actor, m_Text);
        }
    }
}
