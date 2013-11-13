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

    using System.Collections.Generic;

    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;
    using Artemis.Utils;

    using Microsoft.Xna.Framework;

    using TTengine.Comps;

    #endregion

    /// <summary>The collision system.</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 1)]
    internal class CollisionSystem : EntitySystem
    {
        /// <summary>Initializes a new instance of the <see cref="CollisionSystem" /> class.</summary>
        public CollisionSystem()
            : base(Aspect.All(typeof(PositionComp),typeof(ShapeComp)))
        {
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            Bag<Entity> allObj = this.EntityWorld.GroupManager.GetEntities(ShapeComp.CollisionGroupName); 
            if (allObj != null )
            {
                for (int i1 = 0; allObj.Count > i1; ++i1)
                {
                    Entity e1 = allObj.Get(i1);
                    for (int i2 = i1+1; allObj.Count > i2; ++i2)
                    {
                        Entity e2 = allObj.Get(i2);

                        if (this.CollisionExists(e1, e2))
                        {

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
            var p1 = entity1.GetComponent<PositionComp>();
            var p2 = entity2.GetComponent<PositionComp>();
            var s1 = entity1.GetComponent<ShapeComp>();
            var s2 = entity1.GetComponent<ShapeComp>();
            p1.Z = 0f;
            p2.Z = 0f;
            float dist = Vector3.Distance(p1.Position, p2.Position);
            return (dist < (s1.Radius + s2.Radius));
            
        }
    }
}