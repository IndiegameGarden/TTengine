// (c) 2010-2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using TTengine.Util;

namespace Game1
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if !DEBUG
            try
            {
            using (var game = new Game1())
                game.Run();
            }
            catch (Exception ex)
            {
                MsgBox.Show("FEIG! (Fatal Error In Game)",
                  "Fatal Error - if you want you can notify the author.\n" + ex.Message + "\n" + ex.ToString());                
            }
#else
            using (var game = new Game1())
                game.Run();
#endif
        }
    }
#endif
}
