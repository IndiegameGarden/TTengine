// (c) 2010-2012 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{
    /// Generic event arguments class to be used for Gamelet related events
    public class GameletEventArgs : EventArgs
    {
        public GameletEventArgs(Gamelet otherItem)
        {
            this.otherItem = otherItem;
        }

        public Gamelet otherItem;
    }

    public delegate void GameletEventHandler(Gamelet sender, GameletEventArgs e);

    /**
     * basic game item class that can contain children items. It may have Shape (which is 
     * a Spritelet). It has simulated motion and provides subclass extendibility via On...() methods.
     * Also defines some eventing.
     * See also: Spritelet
     */
    public class Gamelet: IDisposable
    {

        #region Eventing
        public event GameletEventHandler OnCollisionEvent;
        #endregion
        
        #region Properties

        // get the unique ID of this Gamelet
        public int ID     { get { return _ID; } }
        
        /// Flag indicating if Gamelet should be deleted - if true deletion occurs during next Update
        public bool Delete = false;
        
        /// If false this Gamelet will not update/draw and my children will not update/draw
        public bool Active = true;

        /// <summary>Flag indicating whether visibility is enabled, which means that if true OnDraw() is called</summary>
        public bool Visible = true; 

        public Vector2 Position = Vector2.Zero;
        public Vector2 PositionModifier = Vector2.Zero;
        public virtual Vector2 PositionAbs
        {
            get
            {
                if (Parent == null)
                    return Position + PositionModifier;
                else
                {
                    Vector2 p = (LinkedToParent ? Parent.PositionAbs : Parent.Parent.PositionAbs);
                    p += ((Position + PositionModifier) - Parent.ZoomCenter) * Parent.Zoom + Parent.ZoomCenter;
                    //return ((Position + PositionModifier + (LinkedToParent ? Parent.PositionAbs : Parent.Parent.PositionAbs)) - Parent.ZoomCenter) * Parent.Zoom + Parent.ZoomCenter;
                    //return Position + PositionModifier + (LinkedToParent ? Parent.PositionAbs : Parent.Parent.PositionAbs);
                    return p;
                }
            }
        } // FIXME do a realtime calc based on parents compound value so far.
        
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
        public virtual float RotateAbs { 
            get {
                if (Parent == null)
                    return Rotate + RotateModifier ; 
                else
                    return Rotate + RotateModifier + (LinkedToParent ? Parent.RotateAbs : Parent.Parent.RotateAbs); 
                } 
        }
        
        public float Scale = 1f;
        public float Zoom = 1f;
        public Vector2 ZoomCenter = Vector2.Zero;
        public float ScaleModifier = 1f;
        public virtual float ScaleAbs
        { 
            get {
                if(Parent == null)
                    return Scale * ScaleModifier ; 
                else
                    return Scale * ScaleModifier * (LinkedToParent ? Parent.ScaleAbs : Parent.Parent.ScaleAbs ); 
            } 
        }
        public virtual float ZoomAbs
        {
            get
            {
                if (Parent == null)
                    return Zoom;
                else
                    return Zoom * (LinkedToParent ? Parent.ZoomAbs : Parent.Parent.ZoomAbs);
            }
        }

        /// 2D acceleration vector in normalized coordinates
        public Vector2 Acceleration = Vector2.Zero;

        /// 2D velocity vector in normalized coordinates
        public Vector2 Velocity = Vector2.Zero;
        
        /// If true, my position/rotation/scale will be relative to the parent's pos/rot/scale. If false, not. True by default.
        public bool LinkedToParent = true; 

        /// My parent item, or null if none (in that case I am root or not attached yet)
        public Gamelet Parent = null ;

        public List<Gamelet> Children = new List<Gamelet>();

        /// Color for drawing shape/sprite, setting it will replace Alpha value with DrawColor.A
        public Color DrawColor
        {
            get { return drawColor; }
            set { drawColor = value; }
        }

        /// Alpha value for the DrawColor of this Spritelet, range 0-1, replacing whatever was in DrawColor.A
        public float Alpha
        {
            get { return drawColor.A / 255.0f; }
            set { drawColor.A = (byte)(value * 255.0f); }
        }

        /// a value indicating drawing depth of sprite 0f (front)....1f (back)
        public float LayerDepth = 0.5f;

        /// If set to non-zero, item will auto-delete after simulating for specified duration time
        public float Duration { get { return duration; } set { duration = value; } }

        /// If set to non-zero, item will initially pause until the set simTime is reached
        public float StartTime { 
            get { return startTime; } 
            set { 
                startTime = value;
                if (startTime > 0f)
                    Active = false; // initially paused until startTime reached.
            } 
        }

        /// <summary>
        /// to which Screenlet the item belongs (e.g. where a shape will draw itself). Also non-drawables may use this info.
        /// Null if not set yet or unknown.
        /// </summary>
        public Screenlet Screen = null;

        /// <summary>
        /// total cumulative amount of simulation time of this specific item (i.e. time being Active)
        /// </summary>
        public float SimTime = 0f;

        #endregion

        #region Constructors
        /// <summary>creates Gamelet without shape and position Zero. Can be used to create a root item or container item</summary>
        public Gamelet()
        {
            CreateID();
            Screen = TTengineMaster.ActiveScreen;
            OnInit();
        }

        /// <summary>create gamelet that is only active in the specified state; useful for state machines</summary>
        public Gamelet(IState activeInState)
        {            
            this.activeInState = activeInState;
            CreateID();
            Screen = TTengineMaster.ActiveScreen;
            OnInit();
        }
        #endregion

        #region Internal Vars
        internal Color drawColor = Color.White;
        internal float duration = -1f;
        internal float startTime = 0f;
        private int _ID = -1;
        private static int _IDcounter = 0;
        private IState activeInState = null;
        private IState myState = null;
        #endregion

        #region Overridable Handler methods (On...() methods)

        /// <summary>
        /// called very first on Gamelet construction, even earlier than your custom Gamelet's constructor!
        /// </summary>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        /// called when parent of this Gamelet changes, check this.Parent for new parent.
        /// </summary>
        protected virtual void OnNewParent()
        {
        }

        /// <summary>
        /// called on update, may be overridden to define custom Update behaviour
        /// </summary>
        protected virtual void OnUpdate(ref UpdateParams p)
        {
        }

        /// <summary>
        /// Called when this item is drawn (only if Item is Visible and Active)
        /// </summary>
        protected virtual void OnDraw(ref DrawParams p)
        {
        }

        /// <summary>
        /// Called on moment of Gamelet deletion
        /// </summary>
        protected virtual void OnDelete()
        {
        }

        /// <summary>
        /// Collision may imply that me or a parent of me collided
        /// </summary>
        public virtual void OnCollision(Spritelet withItem)
        {
        }

        #endregion

        #region User methods
        public void SetNextState(IState st)
        {
            if(myState != null)
                myState.OnExit(this);
            myState = st;
            myState.OnEntry(this);
        }

        public IState GetState()
        {
            return myState;
        }

        /// <summary>
        /// checks whether the Gamelet is in indicated state s. State is determined by IState s CLASS not specific instances.
        /// If Gamelet has no state (ie state==null), then state is determined by my Parent or Parent's Parent etc., recursively up the Gamelet tree.
        /// </summary>
        /// <param name="s">an instance of a class with IState interface, the CLASS will be compared to this.GetState()</param>
        public bool IsInState(IState s)
        {
            if (myState == null)
            {
                if (Parent == null)
                    return false;
                else
                    return Parent.IsInState(s);
            }
            Type sType = s.GetType();
            Type myType = myState.GetType();
            return (myState == s || (sType.IsAssignableFrom(myType) && myType.IsAssignableFrom(sType)));
        }

        /// <summary>
        /// Adds a Gamelet as child at the _end_ (back) of the children's list this.Children. Does not add
        /// if 'child' is already a child Gamelet of this one.
        /// </summary>
        public void Add(Gamelet child)
        {
            if (!Children.Contains(child))
            {
                child.Parent = this;
                Children.Add(child);
                child.OnNewParent();
            }
        }

        /// <summary>
        /// Adds (inserts) a Gamelet as child in _begin_ i.e. front of the children's list this.Children
        /// </summary>
        public void AddFront(Gamelet child)
        {
            Children.Insert(0,child);
            child.Parent = this;
            child.OnNewParent();
        }

        /// <summary>
        /// Removes first occurrence of child gamelet from the list of children. The child.OnNewParent() is
        /// called with a null parameter as a result.
        /// </summary>
        /// <returns>true if item is successfully removed; otherwise, false. </returns>
        public bool Remove(Gamelet child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.Parent = null;
                child.OnNewParent();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Implements IDisposable, to instantly stop this Gamelet and its children and free up all unmanaged resources
        /// </summary>
        public virtual void Dispose()
        {
            for (int i = 0; i < Children.Count; i++ )
            {
                Children[i].Dispose();
            }
            Active = false;
            Visible = false;            
        }

        protected void VertexShaderInit(Effect eff)
        {
            // vertex shader init            
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Screen.WidthPixels, Screen.HeightPixels, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            Matrix m = halfPixelOffset * projection;
            EffectParameter mtPar = eff.Parameters["MatrixTransform"];
            if (mtPar != null)
                mtPar.SetValue(m);
        }

        /// <summary>
        /// translate a relative coordinate to pixel coordinates, in the context of this Gamelet (with possibly Zoom applied)
        /// </summary>
        /// <param name="pos">relative coordinate to translate</param>
        /// <returns>translated to pixels coordinate</returns>
        internal Vector2 ToPixels(Vector2 pos)
        {
            //return (pos * Screen.screenHeight - Center) * Zoom + Center;
            return pos * Screen.screenHeight;
        }

        public Vector2 ToPixelsNS(Vector2 pos)
        {
            return pos * Screen.screenHeight;
        }

        // FIXME?
        public float ToPixels(float coord)
        {
            return coord * Screen.screenHeight ;
        }

        #endregion

        #region Private (internal) methods

        internal Vector2 ToPixels(float x, float y)
        {
            return ToPixels(new Vector2(x, y));
        }

        internal Vector2 ToPixelsNS(float x, float y)
        {
            return ToPixelsNS(new Vector2(x, y));
        }

        internal float ToNormalizedNS(float coord)
        {
            return coord * Screen.scalingToNormalized;
        }

        /// <summary>Find the screen that this item should render to (by recursively looking upward in tree)</summary>
        private Screenlet FindScreen()
        {
            if (this is Screenlet) return (this as Screenlet);
            if (Parent == null) return null;
            return Parent.FindScreen();
        }

        private void CreateID()
        {
            // set my unique id
            _ID = _IDcounter;
            _IDcounter++;
        }

        internal virtual void Update(ref UpdateParams p)
        {
            // check if startTime for this object already reached (if any), if yes activate it
            if (!Active && (startTime > 0f) && (p.simTime >= startTime))
                Active = true;
            if (!Active) return;

            // check active in state
            if (activeInState != null)
            {
                if (!IsInState(activeInState))
                    return;
            }

            // simulate object and children
            //Remove any items that need deletion etc
            int i = 0;
            while (i < Children.Count)
            {
                // deleted items
                if (Children[i].Delete)
                {
                    Children[i].DeleteItem();
                    Children.RemoveAt(i);
                }
                // remove items from my tree that were transferred to another parent
                else if (Children[i].Parent != this)
                    Children.RemoveAt(i);
                else
                    i++;
            }

            // reset back the Modifiers, each Update round
            PositionModifier = Vector2.Zero;
            ScaleModifier = 1.0f;
            RotateModifier = 0.0f;

            // simple physics simulation (fixed timestep assumption)
            SimTime += p.dt;
            Position += Vector2.Multiply(Velocity, p.dt);
            Velocity += Vector2.Multiply(Acceleration, p.dt);

            // check if deletion is needed based on duration propertyName of item
            if (duration > 0)
            {
                if (SimTime >= duration)
                    Delete = true;
            }

            //Update each child item. Note each child _may_ modify updPars.
            foreach (Gamelet item in Children)
            {
                item.Update(ref p);
            }

            // finally call custom update handler of current object and its state, if any
            OnUpdate(ref p);
            if (myState != null)
                myState.OnUpdate(this);

        }

        /// Called when item should draw itself. A Gamelet typically does not draw itself but a Spritelet may (e.g. a child of this).
        internal virtual void Draw(ref DrawParams p)
        {
            if (!Active) return;

            // check active in state
            if (activeInState != null)
            {
                if (!IsInState(activeInState))
                    return;
            }

            // render this item
            if (Visible)
                OnDraw(ref p);
            
            // then render all of the child nodes
            foreach (Gamelet item in Children)
            {
                item.Draw(ref p);
            }
        }

        /// Called for deletion of this item - includes deletion of all children and calling OnDeletion
        private void DeleteItem()
        {
            int i = 0;
            while (i < Children.Count)
            {
                Children[i].Delete = true;
                Children[i].DeleteItem();
                i++;
            }
            Children.Clear();
            OnDelete();
        }

        internal void OnCollideEventNotification(Spritelet s)
        {
            // event notifs to subscribers of OnCollision event
            if (OnCollisionEvent != null)
                OnCollisionEvent(this, new GameletEventArgs(s));
        }

        #endregion
    }
}
