// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TTengine.Core
{
    /// <summary>
    /// the Master is a top-level entry point to TTengine
    /// </summary>
    public class TTengineMasterx
    {
        private static Game activeGame = null;
        private static Screenlet activeScreen = null;
        private static List<Screenlet> screenlets = new List<Screenlet>();

        /// <summary>
        /// the Game class that is active (singleton)
        /// </summary>
        public static Game ActiveGame
        {
            get { return activeGame; }
        }
        
        /// <summary>
        /// the Screenlet that is active, by default the last-created one. Gamelets newly created will attach/draw to here by default.
        /// </summary>
        /*
        public static Screenlet ActiveScreen
        {
            get { return activeScreen; }
            set { activeScreen = value; }
        }*/

        /*
        internal static void AddScreenlet(Screenlet s)
        {
            screenlets.Add(s);
            activeScreen = s;
        }

        /// <summary>
        /// create and init the TTengine for the Game class. Can only be used for one Game class at a time.
        /// </summary>
        /// <param name="game"></param>
        public static void Create(Game game)
        {
           activeGame = game;
           activeScreen = null;
           screenlets.Clear();
        }
         */

    }
}
