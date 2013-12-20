using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
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

        /// <summary>Create a Channel with a new empty World and output to default screen</summary>
        internal Channel(int width, int height)
        {
            this.Children = new List<Channel>();
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
            this.Screen = new ScreenComp(width, height);
            var e = this.World.CreateEntity();
            e.AddComponent(this.Screen);
            e.Refresh();
        }

        internal Channel()
        {
            this.Children = new List<Channel>();
            this.World = new EntityWorld();
            this.World.InitializeAll(true);
            this.Screen = new ScreenComp();
            var e = this.World.CreateEntity();
            e.AddComponent(this.Screen);
            e.Refresh();
        }

        public void AddChild(Channel ch)
        {
            Children.Add(ch);
            TTFactory.BuildTo(ch);
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

        internal void Update()
        {
            if (!IsActive)
                return;

            foreach (Channel c in Children)
            {
                c.Update();
            }            
            this.World.Update();
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

            if (Screen.IsVisible)
            {
                TTGame.Instance.GraphicsDevice.SetRenderTarget(Screen.RenderTarget);
                if (Screen.RenderTarget != null)
                    TTGame.Instance.GraphicsDevice.Clear(Screen.BackgroundColor);
                TTGame.Instance.DrawScreen = Screen;
                this.World.Draw();
            }

        }
    }
}
