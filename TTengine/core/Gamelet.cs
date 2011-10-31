// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

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
    public class Gamelet : List<Gamelet>
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

        /// Flag indicating whether visibility is enabled, which means that if true OnDraw() is called
        public bool Visible = true; 

        public Vector2 Position = Vector2.Zero;
        public Vector2 PositionModifier = Vector2.Zero;
        public virtual Vector2 PositionAbsolute
        {
            get
            {
                if(Parent==null)
                    return Position + PositionModifier ;
                else
                    return Position + PositionModifier + (LinkedToParent ? Parent.PositionAbsolute : Parent.Parent.PositionAbsolute );
            }
        } // FIXME do a realtime calc based on parents compound value so far.


        public float Rotate = 0f;
        public float RotateModifier = 0f;
        public virtual float RotateAbsolute { 
            get {
                if (Parent == null)
                    return Rotate + RotateModifier ; 
                else
                    return Rotate + RotateModifier + (LinkedToParent ? Parent.RotateAbsolute : Parent.Parent.RotateAbsolute); 
                } 
        }
        
        public float Scale = 1f;
        public float ScaleModifier = 1f;
        public virtual float ScaleAbsolute
        { 
            get {
                if(Parent == null)
                    return Scale * ScaleModifier ; 
                else
                    return Scale * ScaleModifier * (LinkedToParent ? Parent.ScaleAbsolute : Parent.Parent.ScaleAbsolute ); 
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
                Active = false; // initially paused until startTime reached.
            } 
        }

        // to which Screenlet the item belongs (e.g. where a shape will draw itself). Also non-drawables may use this info.
        public Screenlet Screen = null;

        // total cumulative amount of simulation time of this specific item (i.e. time being Active)
        public float SimTime = 0f;

        #endregion

        #region Constructors
        /// Default constructor creates an item without shape and position Zero. Can be used to create a root item or container item.
        public Gamelet()
        {
            CreateID();
        }

        /// Create gamelet that is only active in the specified state
        public Gamelet(IState activeInState)
        {
            CreateID();
            this.activeInState = activeInState;
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
        /// called on initialization of game tree
        protected virtual void OnInit()
        {
        }

        /// called when parent changes
        protected virtual void OnNewParent()
        {
        }

        /// called on update, may be overridden to define custom Update behaviour
        protected virtual void OnUpdate(ref UpdateParams p)
        {
        }

        /// Called when this item is drawn (only if Item is Visible and Active)
        protected virtual void OnDraw(ref DrawParams p)
        {
        }

        /// Collision may imply that me or a parent of me collided
        protected virtual void OnDelete()
        {
        }

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

        /**
         * checks whether the Gamelet is in indicated state s. State is determined by CLASS not specific instances.
         * If Gamelet has no state (ie state==null), then state is determined by my Parent or Parent's Parent etc., recursively up the tree.
         */
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

        /// Adds a Gamelet (which could be a shape or behavior, or plain Gamelet etc.) to me as child at the back of the children's list
        public new void Add(Gamelet childItem)
        {
            childItem.Parent = this;
            // call the 'real' add method on the List base class
            ((List<Gamelet>)this).Add(childItem);
            childItem.Screen = childItem.FindScreen();
            childItem.OnNewParent();
        }

        /// Adds (inserts) a Gamelet in front of the children's list
        public void AddFront(Gamelet childItem)
        {
            childItem.Parent = this;
            // call the 'real' method on the List base class
            Insert(0,childItem);
            childItem.Screen = childItem.FindScreen();
            childItem.OnNewParent();
        }

        protected void VertexShaderInit(Effect eff)
        {
            // vertex shader init
            //Viewport viewport = screen.graphicsDevice.Viewport;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Screen.WidthPixels, Screen.HeightPixels, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            Matrix m = halfPixelOffset * projection;
            eff.Parameters["MatrixTransform"].SetValue(m);

        }

        #endregion

        #region Private (internal) methods
        /// Find the screen that this item should render to (by recursively looking upward in tree)
        private Screenlet FindScreen()
        {
            if (this is Screenlet) return (this as Screenlet);
            if (Parent == null) return null;
            return Parent.FindScreen();
        }

        /// Initialize this item and recursively all children underneath, using OnInit() calls
        internal void Initialize()
        {
            Screen = FindScreen();
            OnInit();
            foreach (Gamelet c in this)
                c.Initialize();
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
            while (i < Count)
            {
                // deleted items
                if (this[i].Delete)
                {
                    this[i].DeleteItem();
                    RemoveAt(i);
                }
                // remove items from my tree that were transferred to another parent
                else if (this[i].Parent != this)
                    RemoveAt(i);
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

            // check if deletion is needed based on duration property of item
            if (duration > 0)
            {
                if (SimTime >= duration)
                    Delete = true;
            }

            //Update each child item. Note each child _may_ modify updPars.
            foreach (Gamelet item in this)
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

            //render all of the child nodes
            foreach (Gamelet item in this)
            {
                item.Draw(ref p);
            }

            if (Visible) 
                OnDraw(ref p);
        }

        /// Called for deletion of this item - includes deletion of all children and calling OnDeletion
        private void DeleteItem()
        {
            int i = 0;
            while (i < Count)
            {
                this[i].Delete = true;
                this[i].DeleteItem();
                i++;
            }
            Clear();
            OnDelete();
        }

        private void CreateID()
        {
            // set my unique id
            _ID = _IDcounter;
            _IDcounter++;
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
