// (c) 2010-2012 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    public class Motion: Gamelet
    {
        protected Vector2 targetPos = Vector2.Zero;
        protected bool isTargetSet = false;

        public Vector2 Position = Vector2.Zero;
        public Vector2 PositionModifier = Vector2.Zero;
        /// 2D acceleration vector in normalized coordinates
        public Vector2 Acceleration = Vector2.Zero;
        /// 2D velocity vector in normalized coordinates
        public Vector2 Velocity = Vector2.Zero;
        public virtual Vector2 PositionAbs
        {
            get
            {
                if (Parent.Parent == null || Parent.Parent.Motion == null )
                    return Position + PositionModifier;
                else
                {
                    Vector2 p = (Parent.LinkedToParent ? Parent.Parent.Motion.PositionAbs : Parent.Parent.Parent.Motion.PositionAbs);
                    p += ((Position + PositionModifier) - Parent.Parent.Motion.ZoomCenter) * Parent.Parent.Motion.Zoom + Parent.Parent.Motion.ZoomCenter;
                    //return ((Position + PositionModifier + (LinkedToParent ? Parent.PositionAbs : Parent.Parent.PositionAbs)) - Parent.ZoomCenter) * Parent.Zoom + Parent.ZoomCenter;
                    //return Position + PositionModifier + (LinkedToParent ? Parent.PositionAbs : Parent.Parent.PositionAbs);
                    return p;
                }
            }
        } // FIXME do a realtime calc based on parents compound value so far.

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
        /// absolute drawing position on screen in units of pixels for use in Draw() calls
        /// </summary>
        public virtual Vector2 DrawPosition
        {
            get
            {
                return ToPixels(PositionAbs);
            }
        }


        public float Rotate = 0f;
        public float RotateModifier = 0f;
        public virtual float RotateAbs
        {
            get
            {
                if (Parent.Parent == null)
                    return Rotate + RotateModifier;
                else
                    return Rotate + RotateModifier + (Parent.LinkedToParent ? Parent.Parent.Motion.RotateAbs : Parent.Parent.Parent.Motion.RotateAbs);
            }
        }

        public float Scale = 1f;
        public float Zoom = 1f;
        public Vector2 ZoomCenter = Vector2.Zero;
        public float ScaleModifier = 1f;
        public virtual float ScaleAbs
        {
            get
            {
                if (Parent.Parent == null)
                    return Scale * ScaleModifier;
                else
                    return Scale * ScaleModifier * (Parent.LinkedToParent ? Parent.Parent.Motion.ScaleAbs : Parent.Parent.Parent.Motion.ScaleAbs);
            }
        }
        public virtual float ZoomAbs
        {
            get
            {
                if (Parent.Parent == null)
                    return Zoom;
                else
                    return Zoom * (Parent.LinkedToParent ? Parent.Parent.Motion.ZoomAbs : Parent.Parent.Parent.Motion.ZoomAbs);
            }
        }

        internal override void Update(ref UpdateParams p)
        {
            // reset back the Modifiers, each Update round
            // *before* any children are simulated.
            if (Active)
            {
                PositionModifier = Vector2.Zero;
                ScaleModifier = 1.0f;
                RotateModifier = 0.0f;
            }
            base.Update(ref p);
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);

            // simple physics simulation (fixed timestep assumption)
            // with optional target to move to with given velocity
            if (isTargetSet)
            {
                float vel = Velocity.Length();
                Vector2 vdif = TargetPos - Position;
                Vector2 vmove = vdif;
                vmove.Normalize();
                vmove *= vel * p.Dt;
                if (vmove.LengthSquared() > vdif.LengthSquared())
                {
                    // target reached
                    isTargetSet = false;
                    Position = TargetPos;
                }
                else
                {
                    Position += vmove;
                }
                // adapt velocity normally with acceleration, for next round
                Velocity += Vector2.Multiply(Acceleration, p.Dt);
            }
            else
            {
                Position += Vector2.Multiply(Velocity, p.Dt);
                Velocity += Vector2.Multiply(Acceleration, p.Dt);
            }
        }
    }
}
