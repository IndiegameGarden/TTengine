using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using TTengine.Comps;

namespace TTengine.Core
{
    /// <summary>
    /// A convenience wrapper around an EntityWorld and a Screenlet to which the world renders.
    /// <seealso cref="ChannelManager"/>
    /// </summary>
    public class Channel
    {
        /// <summary>If true, the World of this channel is actively simulated (Updated)</summary>
        public bool IsActive = false;

        /// <summary>If true, the World of this channel is actively rendered (Drawn) to the Screenlet</summary>
        public bool IsVisible = false;

        /// <summary>The Screenlet that this channel renders to</summary>
        public Entity Screenlet;

        /// <summary>The EntityWorld that is being rendered in this channel</summary>
        public EntityWorld World;

        /// <summary>List of child Channels, which are channels rendered within this Channel e.g. sub-screens.</summary>
        public List<Channel> ChildChannels;

        /// <summary>Create a Channel with a new empty World and output to default screen</summary>
        internal Channel(int width, int height)
        {
            this.ChildChannels = new List<Channel>();
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
            this.Screenlet = TTFactory.CreateScreenlet(width, height);
        }

        internal Channel()
        {
            this.ChildChannels = new List<Channel>();
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
            TTFactory.BuildWorld = this.World;
            this.Screenlet = TTFactory.CreateScreenlet();
        }

        /// <summary>
        /// Zap (instantly switch) to this Channel
        /// </summary>
        public void ZapTo() {
            TTGame.Instance.ChannelMgr.ZapTo(this);
        }

        /// <summary>
        /// Fade (slowly change) to this Channel
        /// </summary>
        public void FadeTo()
        {
            throw new NotImplementedException();
        }

        /// <summary>Renders the channel to the associated screenlet</summary>
        internal void Draw()
        {
            // child channels need to be drawn first, as they will be rendered as textures
            // into the current active channel.
            foreach (Channel c in ChildChannels)
            {
                c.Draw();
            }
            var drawScreen = Screenlet.GetComponent<ScreenComp>();
            TTGame.Instance.GraphicsDevice.SetRenderTarget(drawScreen.RenderTarget);
            TTGame.Instance.GraphicsDevice.Clear(drawScreen.BackgroundColor);
            TTGame.Instance.DrawScreen = drawScreen;
            this.World.Draw();
        }
    }
}
