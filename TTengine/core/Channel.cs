// (c) 2010-2015 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Artemis;
using Artemis.System;
using TTengine.Comps;

namespace TTengine.Core
{
    /// <summary>
    /// A convenience wrapper around an EntityWorld and a default ScreenComp to which the world renders.
    /// <seealso cref="ChannelManager"/>
    /// </summary>
    public class Channel
    {
        /// <summary>If true, the World of this channel is actively simulated (Updated)</summary>
        public bool IsActive = true;

        /// <summary>If true, the World of this channel is actively rendered (Drawn)</summary>
        public bool IsVisible = true;

        /// <summary>The screen that this channel renders to by default</summary>
        public ScreenComp Screen;

        /// <summary>The EntityWorld that is being rendered in this channel</summary>
        public EntityWorld World;

        /// <summary>List of child Channels, which are channels rendered within this Channel e.g. sub-screens.</summary>
        public List<Channel> Children;

        /// <summary>
        /// List of post-processing shader effects, applied to entire channel screen
        /// </summary>
        public List<Effect> PostEffects;

        /// <summary>Create a Channel with a new empty World and output to default screen</summary>
        internal Channel(bool hasRenderBuffer, int width, int height)
        {
            this.Children = new List<Channel>();
            this.PostEffects = new List<Effect>();
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
            this.Screen = new ScreenComp(hasRenderBuffer, width, height);
            var e = this.World.CreateEntity();
            e.AddComponent(this.Screen);
            e.Refresh();
        }

        internal Channel(bool hasRenderBuffer, bool hasScreen)
        {
            this.Children = new List<Channel>();
            this.PostEffects = new List<Effect>();
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
            this.Screen = null;
            if (hasScreen)
            {
                this.Screen = new ScreenComp(hasRenderBuffer);
                var e = this.World.CreateEntity();
                e.AddComponent(this.Screen);
                e.Refresh();
            }
        }

        public void AddChild(Channel ch)
        {
            Children.Add(ch);
            TTFactory.BuildTo(ch); // TODO questionable BuildTo
        }

        /// <summary>
        /// Zap (instantly switch) to current Channel, assuming it is a 
        /// child of Root Channel.
        /// </summary>
        public void ZapTo()
        {
            TTGame.Instance.ChannelMgr.Root.ZapTo(this);
        }

        /// <summary>
        /// Zap (instantly switch) to a child Channel, disabling any other childs.
        /// Also rendering for this Channel is disabled to allow child to show.
        /// </summary>
        public void ZapTo(Channel toZapTo) {
            foreach (Channel c2 in Children)
            {
                if (toZapTo == c2)
                {
                    c2.IsActive = true;
                    c2.IsVisible = true;
                }
                else
                {
                    c2.IsActive = false;
                    c2.IsVisible = false;
                }
            }
            //this.Screen.IsVisible = false;
        }

        /// <summary>
        /// Fade (slowly change) to this Channel
        /// </summary>
        public void FadeTo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enable an EntitySystem in this Channel and all child channels
        /// </summary>
        /// <typeparam name="T">System to enable</typeparam>
        public void EnableSystem<T>() where T: EntitySystem
        {
            World.SystemManager.GetSystem<T>().IsEnabled = true;
            foreach (Channel c in Children)
                c.EnableSystem<T>();
        }

        /// <summary>
        /// Disable an EntitySystem in this Channel and all child channels
        /// </summary>
        /// <typeparam name="T">System to disable</typeparam>
        public void DisableSystem<T>() where T : EntitySystem
        {
            World.SystemManager.GetSystem<T>().IsEnabled = false;
            foreach (Channel c in Children)
                c.DisableSystem<T>();
        }

        internal void Update(long deltaTicks)
        {
            if (!IsActive)
                return;

            foreach (Channel c in Children)
            {
                c.Update(deltaTicks);
            }            
            this.World.Update(deltaTicks);
        }

        /// <summary>Renders the channel to the associated screen(s)</summary>
        internal void Draw()
        {
            if (!IsVisible)
                return;

            // child channels need to be drawn first, as they will be rendered as textures
            // into the current active channel.
            foreach (Channel c in Children)
            {
                c.Draw();
            }

            if (Screen == null)
            {
                // a screenless world can still be drawn - to render the audio e.g.
                this.World.Draw();
            }
            else if (Screen.IsVisible)
            {
                TTGame.Instance.GraphicsDevice.SetRenderTarget(Screen.RenderTarget);
                TTGame.Instance.GraphicsDevice.Clear(Screen.BackgroundColor);
                TTGame.Instance.DrawScreen = Screen;
                this.World.Draw();

                // apply post-effects
                if (PostEffects.Count > 0)
                {
                    RenderTarget2D currentSourceBuffer = Screen.RenderTarget;
                    RenderTarget2D currentTargetBuffer = Screen.RenderTargetBackBuffer;
                    SpriteBatch spriteBatch = Screen.SpriteBatch;
#if DEBUG
                    if (currentSourceBuffer == null) throw new Exception("PostEffects can only be used on Screenlets with a RenderTarget2D buffer set");
#endif
                    foreach (Effect eff in PostEffects)
                    {
                        TTGame.Instance.GraphicsDevice.SetRenderTarget(currentTargetBuffer);
                        // TODO could use Screen.spriteBatch.BeginParameterized()
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, eff);
                        spriteBatch.Draw(currentSourceBuffer, new Rectangle(0, 0, Screen.Width, Screen.Height), Color.White);
                        spriteBatch.End();

                        // Swap trick! for a next round.
                        RenderTarget2D temp = currentSourceBuffer;
                        currentSourceBuffer = currentTargetBuffer;
                        currentTargetBuffer = temp;
                    }
                    Screen.renderTarget = currentSourceBuffer;
                    Screen.renderTargetBackBuffer = currentTargetBuffer;

                    /* ?
                    TTGame.Instance.GraphicsDevice.SetRenderTarget(null);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, eff);
                    spriteBatch.Draw(currentSourceBuffer, new Rectangle(0, 0, Screen.Width, Screen.Height), Color.White);
                    spriteBatch.End();
                     */
                }
            }

        }
    }
}
