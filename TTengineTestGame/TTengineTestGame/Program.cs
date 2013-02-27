// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
using System;

namespace TTengineTestGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

