using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Comps
{
    /// <summary>Animated sprite based on a sprite atlas bitmap</summary>
    public class AnimatedSpriteComp: SpriteComp
    {
        public int CurrentFrame { get; set; }

        public int TotalFrames
        {
            get
            {
                return totalFrames;
            }
        }

        internal int totalFrames = 0, px, py, Nx, Ny ;

        /// <summary>
        /// Create new, loading from atlas bitmap file
        /// </summary>
        /// <param name="spriteAtlasBitmapFile">sprite atlas bitmap file with animation sequence in a Nx-by-Ny grid</param>
        /// <param name="Nx">Number of sprite images horizontally (X direction)</param>
        /// <param name="Ny">Number of sprite images vertically (Y direction)</param>
        public AnimatedSpriteComp(string spriteAtlasBitmapFile, int Nx, int Ny) :
            base(spriteAtlasBitmapFile)
        {
            this.Nx = Nx;
            this.Ny = Ny;
            px = Texture.Width / Nx;
            py = Texture.Height / Ny;
            totalFrames = Nx * Ny;
        }

        protected override void InitTextures()
        {
            base.InitTextures();
            CurrentFrame = 0;
            if (Nx > 0 && Ny > 0)
            {
                px = Texture.Width / Nx;
                py = Texture.Height / Ny;
            }
        }
    }
}
