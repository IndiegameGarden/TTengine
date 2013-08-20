// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengineTestGame
{
    public class Ball: Gamelet
    {
        public Ball()
        {
            ConstructSpritelet("ball");
            TimingComp.AddTo(this);
            CollisionComp.AddCollision(this);
        }

        public override void OnInit()
        {
            // FIXME separate obj; add headers to files
            Sprite.ChecksCollisions = true;
            Sprite.Radius *= 0.9f;            
        }

        public override void OnCollision(Gamelet withItem)
        {
            // swap the velocity vecs - pseudo-phyics effect
            Vector2 v = withItem.Motion.Velocity;
            withItem.Motion.Velocity = Motion.Velocity;
            Motion.Velocity = v;
        }

    }
}
