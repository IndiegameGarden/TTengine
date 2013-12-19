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
        /// The currently selected channel. 
        /// </summary>
        public Channel SelectedChannel
        {
            get
            {
                return _selectedChannel;
            }
        }

        /// <summary>
        /// Currently available channels in the manager
        /// </summary>
        public List<Channel> Channels = new List<Channel>();

        protected TTGame _game = null;
        protected Channel _selectedChannel = null;

        internal ChannelManager(TTGame game)
        {
            _game = game;
        }

        /// <summary>
        /// 'zaps' to a Channel i.e. makes it the active one for rendering, de-activating any others.
        /// </summary>
        /// <param name="c">Channel to zap to</param>
        internal void ZapTo(Channel c)
        {
            foreach (Channel c2 in Channels)
            {
                c2.IsActive = false;
                c2.IsVisible = false;
            }
            c.IsActive = true;
            c.IsVisible = true;
            this._selectedChannel = c;        
        }

    }
}
