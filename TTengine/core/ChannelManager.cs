using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Core
{
    /// <summary>
    /// Manages the Channels of a TTGame. A Channel shows content in an EntityWorld
    /// and can be anything like a title screen, a level, a cutscene screen, a highscore screen, or an end screen.
    /// </summary>
    public class ChannelManager
    {
        protected TTGame _game = null;

        internal ChannelManager(TTGame game)
        {
            _game = game;
        }

        public List<Channel> Channels = new List<Channel>();


        /// <summary>
        /// Create a new Channel, registers it within the current TTGame.
        /// </summary>
        /// <returns></returns>
        public Channel CreateChannel()
        {
            Channel c = new Channel(_game);
            _game.ChannelMgr.Add(c);
            return c;
        }

        public void Add(Channel c)
        {
            if (!Channels.Contains(c))
                Channels.Add(c);
        }

        /// <summary>
        /// 'zaps' to a Channel i.e. makes it the active one, de-activating any others.
        /// </summary>
        /// <param name="c"></param>
        public void ZapTo(Channel c)
        {
            foreach (Channel c2 in Channels)
                c2.IsActive = false;
            c.IsActive = true;
            // TODO: the soft fades etc
            _game.ActiveScreen = c.Screen;
            _game.ActiveWorld = c.World;
        }

    }
}
