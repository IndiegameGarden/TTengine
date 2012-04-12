// (c) 2010-2012 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /// <summary>
    /// contains all elements to provide motion (position, velocity, scale, rotation, zoom, etc.) to a gamelet
    /// </summary>
    public class Motion: Gamelet
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 PositionModifier = Vector2.Zero;
        /// 2D acceleration vector in normalized coordinates
        public Vector2 Acceleration = Vector2.Zero;
        /// 2D velocity vector in normalized coordinates
        public Vector2 Velocity = Vector2.Zero;
        /// <summary>
        /// If true, my position/rotation/scale will be relative to the parent's pos/rot/scale. If false, not. True by default.
        /// </summary>
        public Motion MotionParent
        {
            get
            {
                if (motionParent == null)
                {
                    if (Parent.Parent.Motion == null)
                        return null;
                    else
                        return Parent.Parent.Motion;
                }
                return motionParent;
            }
            set
            {
                motionParent = value;
            }
        }

        public virtual Vector2 PositionAbs
        {
            get
            {
                if (MotionParent == null )
                    return Position + PositionModifier;
                else
                    return MotionParent.PositionAbs + Position + PositionModifier ;
            }
        } // FIXME do a realtime calc based on parents compound value so far.

        /// <summary>
        /// return the position for drawing in screen coordinates (after zoom applied etc.)
        /// FIXME rename with DrawPosition 
        /// </summary>
        public virtual Vector2 PositionDraw
        {
            get
            {
                Vector2 p = (PositionAbs - MotionParent.ZoomCenter) * MotionParent.Zoom + MotionParent.ZoomCenter;
                return p;
            }
        }

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

        /// <summary>
        /// absolute drawing position on screen in units of pixels for use in Draw() calls
        /// </summary>
        public virtual Vector2 DrawPosition
        {
            get
            {
                return ToPixels(PositionDraw);
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
                    return Rotate + RotateModifier + MotionParent.RotateAbs;
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
                    return Scale * ScaleModifier * MotionParent.ScaleAbs;
            }
        }
        public virtual float ZoomAbs
        {
            get
            {
                if (Parent.Parent == null)
                    return Zoom;
                else
                    return Zoom * MotionParent.ZoomAbs;
            }
        }

        /// <summary>
        /// set target for Scale
        /// </summary>
        public float ScaleTarget = 1.0f;

        /// <summary>
        /// speed for scaling towards ScaleTarget
        /// </summary>
        public float ScaleSpeed = 0f;

        // zoom, scale etc. related vars for panel
        public float ZoomTarget = 1.0f;

        public float ZoomSpeed = 0f;

        public Motion ZoomCenterTarget = null;

        public float RotateTarget = 0f;

        public float RotateSpeed = 0f;

        
        protected Vector2 targetPos = Vector2.Zero;
        protected bool isTargetSet = false;

        /// <summary>
        /// if null, means use default parent ("one up")
        /// </summary>
        protected Motion motionParent = null;

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
                // motion towards target
                //Velocity = (TargetPos - Position) * 0.01f;
                // FIXME allow to choose linear vs 'smoothed' motion mode???
                MoveToTarget(ref p, false);
            }
            else
            {
                Position += Vector2.Multiply(Velocity, p.Dt);
                Velocity += Vector2.Multiply(Acceleration, p.Dt);
            }

            // handle scaling over time
            ScaleToTarget(ScaleTarget, ScaleSpeed, ScaleSpeed * 0.01f); // FIXME ref p or empty? no 0.01 , scalespeed consider

            // handle dynamic zooming
            ZoomToTarget(ref p);

            // rotation
            RotateToTarget();

        }

        protected void MoveToTarget(ref UpdateParams p, bool isLinearMotionMode)
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
                        vmove *= vel * p.Dt;
                    }
                    else
                    {
                        vmove *= vel * p.Dt;
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
            Velocity += Vector2.Multiply(Acceleration, p.Dt);
        }

        protected void ZoomToTarget(ref UpdateParams p)
        {
            // handle dynamic zooming
            if (ZoomSpeed > 0f)
            {
                // handle zoom value
                if (Zoom < ZoomTarget)
                {
                    Zoom *= (1.0f + ZoomSpeed);
                    if (Zoom > ZoomTarget)
                        Zoom = ZoomTarget;
                }
                else if (Zoom > ZoomTarget)
                {
                    Zoom /= (1.0f + ZoomSpeed);
                    if (Zoom < ZoomTarget)
                        Zoom = ZoomTarget;
                }

                // handle zoom center moving                
                if (ZoomCenterTarget != null && !ZoomCenter.Equals(ZoomCenterTarget.Position))
                {
                    ZoomCenter = ZoomCenterTarget.PositionAbs;
                  /*
                    Vector2 vdif = ZoomCenterTarget.PositionAbs - ZoomCenter;
                    float vel = 1000.0f * ZoomSpeed * vdif.Length();
                    if (vel < ZoomSpeed * 100.0f)
                        vel = ZoomSpeed * 100.0f;
                    Vector2 vmove = vdif;
                    vmove.Normalize();
                    vmove *= vel * p.Dt;
                    if (vmove.LengthSquared() > vdif.LengthSquared())
                    {
                        // target reached
                        ZoomCenter = ZoomCenterTarget.PositionAbs; // FIXME abs?
                    }
                    else
                    {
                        ZoomCenter += vmove;
                    }
                 */
                }

            }
            // Screen.Center; // TODO add Screen.Origin? Screen.TopRightCorner?

        }

        protected void RotateToTarget()
        {
            // handle dynamic zooming
            if (RotateSpeed > 0f)
            {
                if (Rotate < RotateTarget)
                {
                    Rotate += (RotateSpeed);
                    if (Rotate > RotateTarget)
                        Rotate = RotateTarget;
                }
                else if (Rotate > RotateTarget)
                {
                    Rotate -= (RotateSpeed);
                    if (Rotate < RotateTarget)
                        Rotate = RotateTarget;
                }
            }
        }

        // scaling logic during OnUpdate()
        protected void ScaleToTarget(float targetScale, float spd, float spdMin)
        {
            if (spd > 0)
            {
                if (Scale < targetScale)
                {
                    Scale += spdMin + spd * (targetScale - Scale); //*= 1.01f;
                    if (Scale > targetScale)
                    {
                        Scale = targetScale;
                    }
                }
                else if (Scale > targetScale)
                {
                    Scale += -spdMin + spd * (targetScale - Scale); //*= 1.01f;
                    if (Scale < targetScale)
                    {
                        Scale = targetScale;
                    }
                }
                if (Parent.DrawInfo != null)
                    Parent.DrawInfo.LayerDepth = 0.8f - Scale / 1000.0f;
            }
        }

    }
}
