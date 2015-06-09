//
// Method IntersectPixels used under Microsoft Permissive License (Ms-PL). (http://create.msdn.com/downloads/?id=15)
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PXengine.Core;
using TreeSharp;
using Artemis;
using Artemis.Interface;

namespace PXengine.Comps
{
    public enum Faction { GOOD, EVIL, NEUTRAL };

    public enum ThingType { HERO, OTHER };

    public class ThingComp: IComponent
    {
        public ThingComp(ThingType type)
        {
            this.Type = type;
        }

        public bool Active = true;

        public bool Visible = true;

        public Color Color = Color.White;

        public Faction Faction = Faction.NEUTRAL;

        public ThingType Type;

        /// <summary>
        /// if true can pass anything without colliding
        /// </summary>
        public bool IsCollisionFree = false;

        /// <summary>
        /// Determines what intensity levels of background pixel color this ThingComp can pass.
        /// Intensity is the sum of R,G,B bytes of pixel. Any background pixel at threshold value or
        /// brighter is passable for this ThingComp.
        /// </summary>
        public int PassableIntensityThreshold;

        /// <summary>
        /// a direction (if any) the Thing is facing towards e.g. up (0,-1), down (0,1) or right (1,0).
        /// </summary>
        public Vector2 FacingDirection = new Vector2(1f, 0f);

        /// <summary>
        /// a 'relative to normal' velocity-of-moving factor i.e. 1f == normal velocity
        /// </summary>
        public double Velocity = 1;

    }

}
