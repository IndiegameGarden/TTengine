#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollisionSystem.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   The collision system.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace TTengine.Systems
{
    #region Using statements

    using System;
    using System.Collections.Generic;

    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;
    using Artemis.Utils;

    using Microsoft.Xna.Framework;

    using TTengine.Comps;

    #endregion

    /// <summary>A collision system for sphere/circular shapes.</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.CollisionSystem)]
    public class SpriteCollisionSystem : EntitySystem
    {
        private List<Entity> allObj = new List<Entity>();

        public SpriteCollisionSystem()
            : base(Aspect.All(typeof(PositionComp),typeof(SpriteComp)))
        {
        }

        public override void OnDisabled(Entity entity)
        {
            base.OnDisabled(entity);
            allObj.Remove(entity);
        }

        public override void OnEnabled(Entity entity)
        {
            base.OnEnabled(entity);
            allObj.Add(entity);
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            int cnt = allObj.Count;
            foreach (Entity e in entities.Values)
            {
                var sc = e.C<SpriteComp>();
                if (sc.IsCheckingCollisions) {
                    sc.Colliders.Clear();
                    foreach (Entity e2 in allObj)
                    {
                        if (e.Equals(e2))
                            continue;
                        if (sc.Colliders.Contains(e2))
                            continue;
                        if (this.CollisionExists(e, e2))
                        {
                            sc.Colliders.Add(e2);
                            e2.C<SpriteComp>().Colliders.Add(e);
                        }
                    }
                }
            }
        }

        /// <summary>The collision exists.</summary>
        /// <param name="entity1">The entity 1.</param>
        /// <param name="entity2">The entity 2.</param>
        /// <returns>The <see cref="bool" />.</returns>
        private bool CollisionExists(Entity entity1, Entity entity2)
        {
            //var p1 = entity1.GetComponent<PositionComp>();
            //var p2 = entity2.GetComponent<PositionComp>();
            var d1 = entity1.C<DrawComp>();
            var d2 = entity2.C<DrawComp>();
            var s1 = entity1.C<SpriteComp>();
            var s2 = entity2.C<SpriteComp>();
            Rectangle r1 = new Rectangle((int)Math.Round(d1.DrawPosition.X-s1.Center.X), 
                                         (int)Math.Round(d1.DrawPosition.Y-s1.Center.Y), s1.Width, s1.Height);
            Rectangle r2 = new Rectangle((int)Math.Round(d2.DrawPosition.X-s2.Center.X), 
                                         (int)Math.Round(d2.DrawPosition.Y-s2.Center.Y), s2.Width, s2.Height);
            return IntersectPixels(r1, s1.TextureData, r2, s2.TextureData);            
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// Method code from http://create.msdn.com/en-US/education/catalog/tutorial/collision_2d_perpixel
        /// used under Microsoft Permissive License (Ms-PL). 
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bounding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }


    }
}