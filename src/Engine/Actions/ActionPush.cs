using RogueSurvivor.Data;
using System;
using System.Drawing;

namespace RogueSurvivor.Engine.Actions
{
    class ActionPush : ActorAction
    {
        readonly MapObject m_Object;
        readonly Direction m_Direction;
        readonly Point m_To;

        public Direction Direction { get { return m_Direction; } }
        public Point To { get { return m_To; } }

        public ActionPush(Actor actor, RogueGame game, MapObject pushObj, Direction pushDir)
            : base(actor, game)
        {
            if (pushObj == null)
                throw new ArgumentNullException("pushObj");

            m_Object = pushObj;
            m_Direction = pushDir;
            m_To = pushObj.Location.Position + pushDir;
        }

        public override bool IsLegal()
        {
            return m_Game.Rules.CanActorPush(m_Actor, m_Object) && m_Game.Rules.CanPushObjectTo(m_Object, m_To, out m_FailReason);
        }

        public override void Perform()
        {
            m_Game.DoPush(m_Actor, m_Object, m_To);
        }
    }
}
