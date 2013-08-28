// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;
using Artemis.Interface;
using TTengine.Core;

namespace TTengine.Comps
{
    /// <summary>
    /// Component for simple physics-based motion 
    /// (velocity, scale, rotation, zoom, etc.)
    /// </summary>
    public class TargetMotionComp : Comp
    {
        public TargetMotionComp()
        {
        }

        public Vector2 Position = Vector2.Zero;

        public Vector2 PositionAbs = Vector2.Zero;

        /// <summary>
        /// 2D acceleration vector in normalized coordinates
        /// </summary>
        public Vector2 Acceleration = Vector2.Zero;

        /// <summary>
        /// 2D velocity vector in normalized coordinates
        /// </summary>
        public Vector2 Velocity = Vector2.Zero;
        
        public Vector2 TargetPos
        {
            get
            {
                return targetPos;
            }
            set
            {
                targetPos = value;
                isTargetSet = true;
            }
        }

        /// <summary>
        /// velocity of moving towards target TargetPos. Setting modifies Velocity.
        /// </summary>
        public float TargetPosSpeed
        {
            get
            {
                return Velocity.Length();
            }
            set
            {
                Velocity = new Vector2(value, 0f);
            }
        }

        protected Vector2 targetPos = Vector2.Zero;
        protected bool isTargetSet = false;

        /*
        protected override void OnUpdate(ref UpdateParams ctx)
        {
            // FIXME ? reset back the Modifiers, each Update round
            // *before* any children are simulated.
            if (Active)
            {
                PositionModifier = Vector2.Zero;
                ScaleModifier = 1.0f;
                RotateModifier = 0.0f;
            }

            // simple physics simulation (fixed timestep assumption)
            // with optional target to move to with given velocity
            if (isTargetSet)
            {
                // motion towards target
                //Velocity = (TargetPos - Position) * 0.01f;
                // FIXME allow to choose linear vs 'smoothed' motion mode???
                MoveToTarget(ref ctx, false);
            }
            else
            {
                Position += Vector2.Multiply(Velocity, ctx.Dt);
                Velocity += Vector2.Multiply(Acceleration, ctx.Dt);
            }

            // handle scaling over time
            ScaleToTarget(ScaleTarget, ScaleSpeed, ScaleSpeed * 0.01f); // FIXME ref ctx or empty? no 0.01 , scalespeed consider

            // handle dynamic zooming
            ZoomToTarget(ref ctx);

            // rotation
            RotateToTarget();

        }
         */

        /*
        protected void MoveToTarget(ref UpdateParams ctx, bool isLinearMotionMode)
        {
            float vel = Velocity.Length();
            if (vel > 0f)
            {
                Vector2 vdif = TargetPos - Position;
                if (vdif.Length() > 0)
                {
                    Vector2 vmove = vdif;
                    if (isLinearMotionMode)
                    {
                        vmove.Normalize();
                        vmove *= vel * ctx.Dt;
                    }
                    else
                    {
                        vmove *= vel * ctx.Dt;
                    }
                    if (vmove.LengthSquared() > vdif.LengthSquared())
                    {
                        // target reached
                        isTargetSet = false;
                        Position = TargetPos;
                        Velocity = Vector2.Zero;
                    }
                    else
                    {
                        Position += vmove;
                    }
                }
            }

            // adapt velocity normally with acceleration, for next round
            Velocity += Vector2.Multiply(Acceleration, ctx.Dt);
        }
        */

    }
}
