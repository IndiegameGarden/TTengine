using System;
using TTengine.Core;
using TTengine.Util;
using Microsoft.Xna.Framework;
using PXengine.Core;
using PXengine.Comps;
using Artemis;

namespace PXengine.Comps
{
    /// <summary>
    /// base class for a Toy that can be picked up, left behind, used (and then it is active for a while), sometimes with 
    /// multiple shots before it is expended. Typically some special power, effect or weapon.
    /// </summary>
    public abstract class Toy: Comp
    {
        public string ToyName = "";

        /// <summary>
        /// if my Parent is a ThingComp, this holds it. If null, there is no Parent which is a ThingComp
        /// </summary>
        public Entity ParentThing = null;

        public bool CanBePickedUp = true;

        /// <summary>
        /// if true this Toy is immediately used (once) by ParentThing upon pickup
        /// </summary>
        public bool UsedUponPickup = true;

        /// <summary>
        /// whether the Toy is being used right now (true) or not (false)
        /// </summary>
        public bool IsUsed = false;

        /// <summary>
        /// whether the Toy usage is being actively triggered now (true) or not (false).
        /// Triggering is for example by keypress or by the AI of a ThingComp.
        /// </summary>
        public bool IsTriggered = false;

        /// <summary>
        /// time this Toy can be used at most (remains active) in a single usage
        /// </summary>
        public float UseTimeMax = 5f;

        /// <summary>
        /// How long the Toy is in use since last activation. 0 if not in use.
        /// </summary>
        public float UseTime = 0f;

        /// <summary>
        /// how many times using this Toy has still left (e.g. 'shots')
        /// </summary>
        public int UsesLeft = 1;

        public Toy()
        {          
        }

        /// <summary>
        /// a ParentThing starts using the Toy
        /// </summary>
        public void StartUsing()
        {
            IsUsed = true;
            UseTime = 0f;
            UsesLeft--;
        }

        /// <summary>
        /// a ParentThing stops using the Toy (or Toy times out)
        /// </summary>
        public void StopUsing()
        {
            UseTime = 0f;
            IsUsed = false;
            IsTriggered = false;
        }

        protected String UsesLeftMsg()
        {
            string s = "";
            if (UsesLeft > 0)
                s = "(" + UsesLeft + " more)";
            return s;
        }

        protected void  OnNewParent()
        {
            /*
             if (Parent is ThingComp)
             {
                 ParentThing = Parent as ThingComp;
                 ParentThing.ToyActive = this;
                 if (UsedUponPickup && UsesLeft > 0)
                 {
                     StartUsing();
                 }
             }
             */
        }

        /*
        protected override void OnUpdate(ref UpdateParams p)
        {
            base.OnUpdate(ref p);
            if(IsUsed)
                UseTime += p.Dt;    
            if (UseTime > UseTimeMax)
            {
                StopUsing();                                
            }

            // check if the last message, if any, about this toy is already done and new instance can be started
            ThingComp pixie = Level.Current.pixie;
            bool pixieFacingToy = CollidesWhenOtherMoves(pixie, pixie.FacingDirection);
            if (toyExplanationMessage != null && toyExplanationMessage.Delete && !pixieFacingToy)
            {
                toyExplanationMessage = null;
            }

            // pixie facing this Toy == print info message
            if (ParentThing == null && toyExplanationMessage==null && pixieFacingToy)
            {
                toyExplanationMessage = Level.Current.Subtitles.Show(5,SayToyName(), 2.1f);
            }

            // collision with pixie = pickup            
            if (ParentThing==null && Collides(pixie))
            {
                if (CanBePickedUp)
                {
                    pixie.Add(this);
                    Visible = false;
                }
                else
                {
                    if (UsesLeft > 0)
                    {
                        StartUsing();
                    }
                }
            }
        }
         */

        // FIXME cant override this method in practice?
        public string SayToyName()
        {
            string tname = ToyName;
            if (tname.Length > 0)
                return "It says:" + (tname.Length > 16 ? "\n" : " ") + "\"" + tname + "\"";
            else
                return "";
        }

    }
}
