
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using TTengine.Core;
using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Provides user control input (mouse, keys, gamepad) information
    /// </summary>
    public class PlayerInputComp: IComponent
    {
        public PlayerInputComp(): this(PlayerIndex.One)
        {
            //
        }

        public PlayerInputComp(PlayerIndex idx)
        {
            Player = idx;
        }

        /// <summary>
        /// Direction of user input
        /// </summary>
        public Vector2 Direction = Vector2.Zero;

        /// <summary>
        /// For which player the input is tracked
        /// </summary>
        public PlayerIndex Player;

    }
}
