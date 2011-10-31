// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using TTengine.Core;

namespace TTengineTestGame
{
    public class Ball: Spritelet
    {
        public Ball()
            : base("ball")
        {
            checksCollisions = true;
            radius *= 0.9f;
        }

        public override void OnCollision(Spritelet withItem)
        {
            base.OnCollision(withItem);

            // swap the velocity vecs - pseudo-phyics eff
            Vector2 v = withItem.Velocity;
            withItem.Velocity = Velocity;
            Velocity = v;
        }
    }
}
