#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementSystem.cs" company="GAMADU.COM">
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
//   The movement system.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace TTengine.Systems
{
    #region Using statements

    using System;

    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;
    using Microsoft.Xna.Framework;
    using TTengine.Comps;

    #endregion

    /// <summary>The movement system.</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = SystemsSchedule.TargetMoveSystem)]
    public class TargetMotionSystem : EntityComponentProcessingSystem<PositionComp, TargetMotionComp>
    {

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(Entity entity, PositionComp posComp, TargetMotionComp targetComp)
        {
            if (!targetComp.isTargetSet)
                return;
            Vector3 v = targetComp.targetPos - posComp.Position;
            if (v.LengthSquared() > 0 ){
                Vector3 vm = v;
                vm.Normalize();
                vm *= (float)(targetComp.TargetVelocity * Dt);
                if (vm.LengthSquared() > v.LengthSquared())
                {
                    // target reached
                    targetComp.isTargetSet = false;
                    posComp.Position = targetComp.targetPos;
                }
                else
                {
                    posComp.Position += vm;
                }
            }
        }
    }

}
