using System;
using System.Collections.Generic;

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
            Root = new Channel(false, false);
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

    }
}
