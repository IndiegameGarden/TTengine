// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{
    /// <summary>
    /// Generic event arguments class to be used for Gamelet related events
    /// </summary>
    public class GameletEventArgs : EventArgs
    {
        /// <summary>
        /// describes item that is subject of event (e.g. colliding Gamelet)
        /// </summary>
        public Gamelet otherItem;

        public GameletEventArgs(Gamelet otherItem)
        {
            this.otherItem = otherItem;
        }
        
    }

    /// <summary>
    /// Generic event handler
    /// </summary>
    /// <param name="sender">generator of event</param>
    /// <param name="e">see above class, which contains any event arguments</param>
    public delegate void GameletEventHandler(Gamelet sender, GameletEventArgs e);

    /**
     * basic game item class that can contain children items. 
     * Provides subclass extendibility via On...() methods.
     * Also defines some (collission) eventing.
     */
    public class Gamelet: Complet
    {

        #region Eventing
        public event GameletEventHandler OnCollisionEvent;
        #endregion
        
        #region Properties

        /// <summary>
        /// Includes position, velocity, any motion behaviors of Gamelet; or null if not motion-capable
        /// </summary>
        public MotionComplet Motion;

        /// <summary>
        /// Information for drawing this Gamelet, or null if not drawable
        /// </summary>
        public DrawComplet DrawInfo;

        /// <summary>
        /// Sprite for this Gamelet, or null if none
        /// </summary>
        public SpriteComplet Sprite;

        /// <summary>
        /// Set the IState in which this Gamelet is Active only. If null (default), it means active in any state.
        /// </summary>
        public IState ActiveInState
        {
            get { return activeInState; }
            set { activeInState = value; }
        }

        /// <summary>
        /// Flag indicating whether visibility is enabled, which means if true OnDraw() is called
        /// </summary>
        public bool Visible = true; 

        public List<Complet> Children = new List<Complet>();

        protected List<Complet> childrenToAdd = new List<Complet>();

        /// If set to non-zero, item will auto-delete after simulating for specified duration time
        public float Duration { get { return duration; } set { duration = value; } }

        /// If set to non-zero, item will initially pause until the set SimTime is reached
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
        
        /// <summary>
        /// creates basic Gamelet that attaches to the currently active Screen
        /// </summary>
        public Gamelet()
        {
            Screen = TTengineMaster.ActiveScreen;
        }

        /// <summary>
        /// turn into a Drawlet
        /// </summary>
        public void CreateDrawlet()
        {
            Motion = new MotionComplet();
            DrawInfo = new DrawComplet();
            Add(Motion);
            Add(DrawInfo);
        }

        /// <summary>
        /// turn into a Spritelet
        /// </summary>
        public void CreateSpritelet()
        {
            CreateDrawlet();
            Sprite = new SpriteComplet((Texture2D) null);
            Add(Sprite);
        }

        /// <summary>
        /// turn into a Spritelet
        /// </summary>
        public void CreateSpritelet(String textureFile)
        {
            CreateDrawlet();
            Sprite = new SpriteComplet(textureFile);
            Add(Sprite);
        }

        /// <summary>
        /// turn into an EffectSpritelet
        /// </summary>
        public void CreateEffectSpritelet(String textureFile, String effectFile)
        {
            CreateDrawlet();
            Sprite = new EffectSpriteComplet(textureFile, effectFile);
            Add(Sprite);
        }

        /// <summary>
        /// turn into an Efflet, TODO only called from efflet class now.
        /// </summary>
        public void CreateEfflet()
        {
            CreateDrawlet();
        }


        #endregion

        #region Internal Vars        
        internal float duration = -1f;
        internal float startTime = 0f;
        private IState activeInState = null;
        private IState myState = null;
        #endregion

        #region Overridable Handler methods (On...() methods)

        /// <summary>
        /// Collision may imply that me or a parent of me collided
        /// </summary>
        public virtual void OnCollision(Gamelet withItem)
        {
            //
        }

        protected override void OnDelete()
        {
            //
        }

        protected override void OnDraw(ref DrawParams p)
        {
            //
        }

        public override void OnNewParent()
        {
            //
        }

        protected override void OnUpdate(ref UpdateParams p)
        {
            //
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
        public void Add(Complet child)
        {
            if (!Children.Contains(child))
            {
                child.Parent = this;
                Children.Add(child);
                child.OnNewParent();
            }
        }

        /// <summary>
        /// add a child gamelet with Add() next update, useful for adding gamelet from another thread
        /// </summary>
        /// <param name="child"></param>
        public void AddNextUpdate(Complet child)
        {
            lock (childrenToAdd)
            {
                childrenToAdd.Add(child);
            }
        }

        /// <summary>
        /// Adds (inserts) a Gamelet as child in _begin_ i.e. front of the children's list this.Children
        /// </summary>
        public void AddFront(Complet child)
        {
            Children.Insert(0,child);
            child.Parent = this;
            child.OnNewParent();
        }

        public void Add(int index, Complet child)
        {
            Children.Insert(index, child);
            child.Parent = this;
            child.OnNewParent();
        }

        /// <summary>
        /// Removes first occurrence of child gamelet from the list of children. The child.OnNewParent() is
        /// called with a null parameter as a result.
        /// </summary>
        /// <returns>true if item is successfully removed; otherwise, false. </returns>
        public bool Remove(Complet child)
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
        public override void Dispose()
        {
            for (int i = 0; i < Children.Count; i++ )
            {
                Children[i].Dispose();
            }
            Visible = false;

            base.Dispose();
        }

        internal void VertexShaderInit(Effect eff)
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
        /// FIXME move away to a component?
        /// translate a float screen coordinate to pixel coordinates, in the context of this Gamelet
        /// </summary>
        /// <param name="pos">relative coordinate to translate</param>
        /// <returns>translated to pixels coordinate</returns>
        public Vector2 ToPixels(Vector2 pos)
        {
            //return (pos * Screen.screenHeight - Center) * Zoom + Center; // TODO check? only for internal?
            return pos * Screen.screenHeight;
        }

        public float ToPixels(float coord)
        {
            return coord * Screen.screenHeight ;
        }

        public float FromPixels(float pixels)
        {
            return pixels / Screen.screenHeight;
        }

        public Vector2 FromPixels(Vector2 pixelCoords)
        {
            return pixelCoords / Screen.screenHeight;
        }

        internal Vector2 ToPixels(float x, float y)
        {
            return ToPixels(new Vector2(x, y));
        }

        #endregion

        #region Private (internal) methods

        /// <summary>Find the screen that this item should render to (by recursively looking upward in tree)</summary>
        private Screenlet FindScreen()
        {
            if (this is Screenlet) return (this as Screenlet);
            if (Parent == null) return null;
            return Parent.FindScreen();
        }

        internal virtual void Update(ref UpdateParams p)
        {
            // check if startTime for this object already reached (if any), if yes activate it
            if (!Active && (startTime > 0f)) {
                float t = Parent.SimTime;
                // finally, check if already time to start, if so activate
                if( t >= startTime)
                    Active = true;
            }
            if (!Active) return;

            // check active in state
            if (activeInState != null)
            {
                if (!IsInState(activeInState))
                    return;
            }

            // advance sim time if Active
            SimTime += p.Dt;

            // add any children that were queued to add
            lock (childrenToAdd)
            {
                foreach (Gamelet g in childrenToAdd)
                    Add(g);
                childrenToAdd.Clear();
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

            // call custom update handler of current object and its state, if any
            OnUpdate(ref p);
            if (myState != null)
                myState.OnUpdate(this, ref p);

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
            {
                OnDraw(ref p);
                if (myState != null)
                    myState.OnDraw(this);
            }
            
            // then render all of the child nodes
            foreach (Gamelet item in Children)
            {
                item.Draw(ref p);
            }
        }

        /// Called for deletion of this item - includes deletion of all children and calling OnDeletion
        internal override void DeleteItem()
        {
            int i = 0;
            while (i < Children.Count)
            {
                Children[i].Delete = true;
                Children[i].DeleteItem();
                i++;
            }
            Children.Clear();

            base.DeleteItem();
        }

        internal void OnCollideEventNotification(Gamelet s)
        {
            // event notifs to subscribers of OnCollision event
            if (OnCollisionEvent != null)
                OnCollisionEvent(this, new GameletEventArgs(s));
        }

        #endregion
    }
}
