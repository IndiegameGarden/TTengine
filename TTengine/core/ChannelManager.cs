using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /// <summary>
    /// Manages the Channels of a TTGame. A Channel shows content in an EntityWorld
    /// and can be anything like a title screenComp, a level, a cutscene screenComp, a highscore screenComp, or an end screenComp.
    /// </summary>
    public class ChannelManager
    {
        /// <summary>
        /// The root channel
        /// </summary>
        public Channel Root = null;

        protected TTGame _game = null;

        internal ChannelManager(TTGame game)
        {
            _game = game;
            Root = new Channel();
            TTFactory.BuildTo(Root);
        }

        /// <summary>
        /// Adds new Channel and selects it as default for building in TTFactory
        /// </summary>
        /// <param name="ch"></param>
        public void AddChannel(Channel ch)
        {
            Root.AddChild(ch);
            TTFactory.BuildTo(ch);
        }

        /// <summary>
        /// 'zaps' to a Channel i.e. makes it the active one for rendering, de-activating any others.
        /// </summary>
        /// <param name="c">Channel to zap to</param>
        internal void ZapTo(Channel c)
        {
            foreach (Channel c2 in Root.Children)
            {
                if (c == c2)
                {
                    c.IsActive = true;
                    c.IsVisible = true;
                }
                else
                {
                    c2.IsActive = false;
                    c2.IsVisible = false;
                }
            }
        }

    }
}
