// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTengine.Core
{
    /**
     * a Spritelet that supports multiple bitmap representations with different Level of Detail (LoD)
     * When drawing, the best-matching bitmap is taken based on current Scale of sprite. The default
     * scale 1.0 is coupled to the first sprite in the array given in the constructor.
     */
    public class LODSpritelet: Spritelet
    {
        private Texture2D[] textures;
        private string[] fileNames;

        public LODSpritelet(string[] fileNames)
        {
            this.fileNames = fileNames;
        }

        protected override void OnInit()
        {
            textures = new Texture2D[fileNames.Length];
            
            for (int i = 0; i < fileNames.Length; i++)
            {
                textures[i] = TTengineMaster.ActiveGame.Content.Load<Texture2D>(fileNames[i]);
            }
            Texture = textures[0];

        }

        protected override void OnDraw(ref DrawParams p)
        {
            // calculate my scale
            float sc = Motion.ScaleAbs;

            // index of best LOD sprite identified to match current scale
            int idxOfBest = 0;
            // scale calculated for the Draw command (may differ from sc due to different-sized LOD sprites)
            float scDraw = 1.0f;
            // if scale not equal to regular 1.0 (which is size of first sprite in LOD array), do a LOD matching operation
            if (sc != 1.0f)
            {
                // calculate desired pixel width of LODsprite
                float pixelsWidth = ToPixels(sc * DrawInfo.Width);

                // find nearest texture with width pixelsWidth
                float lowestErr = float.PositiveInfinity;
                for (int i = 0; i < textures.Length; i++)
                {
                    float err = Math.Abs(textures[i].Width - pixelsWidth);
                    if (err < lowestErr)
                    {
                        lowestErr = err;
                        idxOfBest = i;
                    }
                }
                scDraw = pixelsWidth / textures[idxOfBest].Width;
            }
            // calculate center of current sprite in pixels
            Vector2 ctr = new Vector2(DrawInfo.Center.X * textures[idxOfBest].Width, DrawInfo.Center.Y * textures[idxOfBest].Height);

            MySpriteBatch.Draw(textures[idxOfBest], DrawInfo.DrawPosition, null, DrawInfo.DrawColor,
                    Motion.RotateAbs, ctr, scDraw, SpriteEffects.None, DrawInfo.LayerDepth);
        }
    }
}
