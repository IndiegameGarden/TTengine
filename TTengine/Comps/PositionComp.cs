#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionComp.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   The PositionComp.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace TTengine.Comps
{
    #region Using statements

    using Artemis;
    using Artemis.Attributes;
    using Artemis.Interface;

    using Microsoft.Xna.Framework;
    using TTengine.Core;

    #endregion

    /// <summary>The transform component pool-able.</summary>
    /// just to show how to use the pool =P 
    /// (just add this annotation and extend ArtemisComponentPool =P)
    /// TODO test component pool performance once further in development.
    //[ArtemisComponentPool(InitialSize = 5, IsResizable = true, ResizeSize = 20, IsSupportMultiThread = false)]
    public class PositionComp : Comp //ComponentPoolable
    {
        /// <summary>Initializes a new instance of the <see cref="PositionComp" /> class.</summary>
        public PositionComp()
            : this(Vector3.Zero)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PositionComp" /> class.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public PositionComp(float x, float y, float z)
            : this(new Vector3(x, y, z))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="PositionComp" /> class.</summary>
        /// <param name="position">The position.</param>
        public PositionComp(Vector3 position)
        {
            this.Position = position;
        }

        /// <summary>Gets or sets the position.</summary>
        /// <value>The position.</value>
        public Vector3 Position
        {
            get
            {
                return new Vector3(this.X, this.Y, this.Z);
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
                this.Z = value.Z;
            }
        }

        /// <summary>Gets or sets the X and Y of the position.</summary>
        /// <value>The position.</value>
        public Vector2 Position2D
        {
            get
            {
                return new Vector2(this.X, this.Y);
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public Vector3 PositionModifier = Vector3.Zero;

        /// <summary>Gets or sets the x coordinate.</summary>
        /// <value>The X.</value>
        public float X { get; set; }

        /// <summary>Gets or sets the y coordinate.</summary>
        /// <value>The Y.</value>
        public float Y { get; set; }

        /// <summary>Gets or sets the z coordinate.</summary>
        /// <value>The Z.</value>
        public float Z { get; set; }

        /// <summary>The clean up.</summary>
        public void CleanUp()
        {
            this.Position = Vector3.Zero;
        }
    }
}