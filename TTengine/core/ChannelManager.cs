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
        /// The currently selected channel. It may be modified via the ZapTo method 
        /// or writing this property.
        /// </summary>
        public Channel SelectedChannel
        {
            get
            {
                return _selectedChannel;
            }
            set
            {
                ZapTo(value);
            }
        }

        protected TTGame _game = null;
        protected Channel _selectedChannel = null;

        internal ChannelManager(TTGame game)
        {
            _game = game;
        }

        public List<Channel> Channels = new List<Channel>();


        /// <summary>
        /// Create a new Channel, registers it within the current TTGame, and
        /// selects it as the default TTGame.BuildWorld.
        /// </summary>
        /// <returns></returns>
        public Channel CreateChannel()
        {
            Channel c = new Channel(_game);
            this.Add(c);
            BuildIn(c);
            if (_selectedChannel == null)
                _selectedChannel = c;
            return c;
        }

        // TODO: check when needed to access from outside.
        protected void Add(Channel c)
        {
            if (!Channels.Contains(c))
                Channels.Add(c);
        }

        /// <summary>
        /// 'zaps' to a Channel i.e. makes it the active one for rendering, de-activating any others.
        /// This does not change the TTGame.BuildWorld, which is the world in which new Entities
        /// are created.
        /// </summary>
        /// <param name="c">Channel to zap to</param>
        public void ZapTo(Channel c)
        {
            foreach (Channel c2 in Channels)
            {
                c2.IsActive = false;
                c2.IsVisible = false;
            }
            c.IsActive = true;
            c.IsVisible = true;
            // TODO: the soft fades etc
            this._selectedChannel = c;
        }

        /// <summary>
        /// selects the Channel as the current default channel to build in, to create new
        /// Entities in. TTFactory and any factories based on it, will then use the Channel's World as the world to
        /// build in; and the Channel's Screenlet as the default screen to render to.
        /// </summary>
        /// <param name="c">Channel to build to and render to</param>
        public void BuildIn(Channel c)
        {
            TTFactory.BuildWorld = c.World;
            TTFactory.BuildScreenlet = c.Screen;
        }
    }
}
