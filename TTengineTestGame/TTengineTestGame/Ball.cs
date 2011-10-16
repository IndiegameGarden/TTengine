// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TTengine;

namespace TTengineTestGame
{
    public class Ball: Spritelet
    {
        public Ball()
            : base("ball")
        {
            this.checksCollisions = true;
            this.radius *= 0.9f;
        }

        public override void OnCollision(Spritelet withItem)
        {
            base.OnCollision(withItem);

            // swap the velocity vecs - pseudo-phyics eff
            Vector2 v = withItem.Velocity;
            withItem.Velocity = this.velocity;
            this.velocity = v;

        }

    }
}
