using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTengine.Core;

namespace TTengine.Comps
{
    /// <summary>
    /// Used to renders a screen full of sprites, each sprite color/shape derived from a pixel in a bitmap.
    /// Useful for e.g. level bitmaps. The sprite to render each pixel is taken from SpriteComp; this component defines
    /// the sprite-field.
    /// </summary>
    /// <param name="fieldBitmapFilename">The bitmap defining colors/shapes/positions of all sprites, e.g. the "level" layout</param>
    public class SpriteFieldComp: SpriteComp
    {
        /// <summary>Number of sprites rendered in X direction (i.e. width of intersection taken from total field)</summary>
        public int NumberSpritesX;

        /// <summary>Number of sprites rendered in Y direction (i.e. height of intersection taken from total field)</summary>
        public int NumberSpritesY;

        /// <summary>The topleft corner position of the field that is rendered (i.e. the index coordinate into the field bitmap)</summary>
        public Vector2 FieldPos = Vector2.Zero;

        /// <summary>The number of pixels (horizontal/vertical, i.e. dx/dy) that is between subsequent rendered sprites</summary>
        public Vector2 FieldSpacing = new Vector2(16f, 16f);

        // used internally for quick access to field values
        internal Color[] fieldData;

        public SpriteFieldComp(string fieldBitmapFilename) :
            base(fieldBitmapFilename)
        {            
            //
        }

        // whenever texture changed, get the texture data into 'fieldData' again
        protected override void InitTextures()
        {
            base.InitTextures();
            fieldData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(fieldData);
        }

    }
}
