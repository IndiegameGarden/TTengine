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
    public class Gamelet: TTObject
    {

        #region Eventing
        public event GameletEventHandler OnCollisionEvent;
        #endregion
        
        #region Properties

        /// <summary>
        /// Includes position, velocity, any motion behaviors of Gamelet; or null if not motion-capable
        /// </summary>
        public MotionComp Motion;

        /// <summary>
        /// Information for drawing this Gamelet, or null if not drawable
        /// </summary>
        public DrawComp DrawC;

        /// <summary>
        /// Sprite for this Gamelet, or null if none
        /// </summary>
        public SpriteComp Sprite;

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
            //
        }

        /// <summary>
        /// turn into a Drawlet
        /// </summary>
        public void ConstructDrawlet()
        {
            Motion = new MotionComp();
            DrawC = new DrawComp();
            Add(Motion);
            Add(DrawC);
        }

        /// <summary>
        /// turn into a Spritelet
        /// </summary>
        public void ConstructSpritelet()
        {
            ConstructDrawlet();
            Sprite = new SpriteComp((Texture2D) null);
            Add(Sprite);
        }

        /// <summary>
        /// turn into a Spritelet
        /// </summary>
        public void ConstructSpritelet(String textureFile)
        {
            ConstructDrawlet();
            Sprite = new SpriteComp(textureFile);
            Add(Sprite);
        }

        /// <summary>
        /// turn into an EffectSpritelet
        /// </summary>
        public void ConstructEffectSpritelet(String textureFile, String effectFile)
        {
            ConstructDrawlet();
            Sprite = new ShaderSpriteComp(textureFile, effectFile);
            Add(Sprite);
        }

        /// <summary>
        /// turn into an Efflet, TODO only called from efflet class now.
        /// </summary>
        public void ConstructEfflet()
        {
            ConstructDrawlet();
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

        public override void OnInit()
        {
            //
        }

        public override void OnDraw(ref DrawParams p)
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

        #endregion

        #region Private (internal) methods

        /// <summary>Find the screen that this item should render to (by recursively looking upward in tree)</summary>
        private Screenlet FindScreen()
        {
            if (this is Screenlet) return (this as Screenlet);
            if (Parent == null) return null;
            return Parent.FindScreen();
        }

        internal override void Update(ref UpdateParams p)
        {
            if (!Active) return;

            // check active in state
            if (activeInState != null)
            {
                if (!IsInState(activeInState))
                    return;
            }

            // advance sim time if Active
            SimTime += p.Dt;

            // call custom update handler of current object and its state, if any
            if (myState != null)
                myState.OnUpdate(this, ref p);

            base.Update(ref p);

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
            foreach (TTObject item in Children)
            {
                if (item is Gamelet)
                    (item as Gamelet).Draw(ref p);
                else 
                    item.OnDraw(ref p);
            }
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
