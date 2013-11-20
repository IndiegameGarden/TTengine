using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Comps
{
    public enum AnimationType
    {
        /// <summary>Default forward animation</summary>
        NORMAL,
        /// <summary>Reverse animation sequence</summary>
        REVERSE,
        /// <summary>First forward then reverse animation ('ping pong')</summary>
        PINGPONG
    }

    /// <summary>Animated sprite based on a sprite atlas bitmap</summary>
    public class AnimatedSpriteComp: SpriteComp
    {
        public AnimationType AnimType = AnimationType.NORMAL;

        public int CurrentFrame { get; set; }

        /// <summary>The lowest frame that will be rendered (default 0)</summary>
        public int MinFrame { get; set; }

        /// <summary>The highest frame that will be rendered (default (TotalFrames-1) )</summary>
        public int MaxFrame { get; set; }

        public int TotalFrames
        {
            get
            {
                return totalFrames;
            }
        }

        internal int totalFrames = 0, px, py, Nx, Ny, pingpongDelta=1 ;

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
            this.MinFrame = 0;
            this.CurrentFrame = 0;
            px = Texture.Width / Nx;
            py = Texture.Height / Ny;
            totalFrames = Nx * Ny;
            this.MaxFrame = totalFrames;
        }

        protected override void InitTextures()
        {
            base.InitTextures();
            CurrentFrame = MinFrame;
            if (Nx > 0 && Ny > 0)
            {
                px = Texture.Width / Nx;
                py = Texture.Height / Ny;
            }
        }
    }
}
