// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /// <summary>
    /// Component with elements to provide basic physics-based motion 
    /// (position, velocity, scale, rotation, zoom, etc.) to a gamelet
    /// </summary>
    public class MotionComp : TTObject
    {
        public MotionComp()
        {
            Screen = TTengineMaster.ActiveScreen;
        }

        public Vector2 Position = Vector2.Zero;
        public Vector2 PositionModifier = Vector2.Zero;

        /// <summary>
        /// 2D acceleration vector in normalized coordinates
        /// </summary>
        public Vector2 Acceleration = Vector2.Zero;

        /// <summary>
        /// 2D velocity vector in normalized coordinates
        /// </summary>
        public Vector2 Velocity = Vector2.Zero;
        
        /// <summary>
        /// If a MotionParent exists, my position/rotation/scale will be relative to the parent's pos/rot/scale. 
        /// If null, no motion parent exists.
        /// </summary>
        public MotionComp MotionParent
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

        /// <summary>
        /// the absolute position in normalized coordinates after applying any position modifiers and
        /// possible parent's positions.
        /// TODO do a realtime calc based on parents compound value so far.
        /// </summary>
        public virtual Vector2 PositionAbs
        {
            get
            {
                if (MotionParent == null )
                    return Position + PositionModifier;
                else
                    return MotionParent.PositionAbs + Position + PositionModifier ;
            }
        } 

        /// <summary>
        /// return the absolute position in normalized coordinates after zoom is applied
        /// </summary>
        public virtual Vector2 PositionAbsZoomed
        {
            get
            {
                Vector2 p = (PositionAbs - MotionParent.ZoomCenter) * MotionParent.Zoom + Screen.Center;
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
        /// absolute drawing position on screen in units of pixels for example for use in Draw() calls
        /// Warning: this value is instantaneous and not obtaining using interpolation.
        /// </summary>
        public virtual Vector2 PositionAbsZoomedPixels
        {
            get
            {
                return Parent.DrawC.ToPixels(PositionAbsZoomed);
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

        /// <summary>
        /// the center of zooming actions (expressed in PositionAbs coordinates).
        /// Setting this will reset any ZoomCenterTarget that was set.
        /// </summary>
        public Vector2 ZoomCenter
        {
            get
            {
                if (!isZoomCenterSet)
                    return Screen.Center;
                else
                    return zoomCenter;
            }
            set
            {
                zoomCenter = value;
                ZoomCenterTarget = null;
                isZoomCenterSet = true;
            }
        }

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

        /// <summary>
        /// Specify a ZoomCenter position from the given Motion object i.e. Motion.PositionAbs.
        /// This is overridden by a coordinate setting done on ZoomCenter.
        /// </summary>
        public MotionComp ZoomCenterTarget = null;

        public float RotateTarget = 0f;

        public float RotateSpeed = 0f;

        protected Screenlet Screen = null;
        protected Vector2 targetPos = Vector2.Zero;
        protected bool isTargetSet = false;
        protected Vector2 zoomCenter;
        protected bool isZoomCenterSet = false;

        /// <summary>
        /// if null, means use default parent ("one up")
        /// </summary>
        protected MotionComp motionParent = null;

        protected override void OnUpdate(ref UpdateParams p)
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

        protected override void OnDelete()
        {
            //
        }

        public override void OnDraw(ref DrawParams p)
        {
            //
        }

        public override void OnNewParent(TTObject oldParent)
        {
            //
        }

        public override void OnInit()
        {
            //
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
            }
            if (ZoomCenterTarget != null)
            {
                zoomCenter = ZoomCenterTarget.PositionAbs; // update zoom-center
            }


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
                if (Parent.DrawC != null)
                    Parent.DrawC.LayerDepth = 0.8f - Scale / 1000.0f;
            }
        }

    }
}
