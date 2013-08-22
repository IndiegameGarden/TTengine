// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace TTengine.Comps
{
    /// <summary>
    /// Component for a sprite 
    /// </summary>
    public class SpriteComp : IComponent
    {

        #region Constructors

        /// <summary>
        /// create new sprite where Texture is loaded from the content with given fileName, either XNA content
        /// or a separately loaded bitmap at runtime
        /// </summary>
        /// <param name="fileName">name of XNA content file (from content project) without file extension e.g. "test", or
        /// name of bitmap file to load including extension e.g. "test.png"</param>
        /// <exception cref="InvalidOperationException">when invalid image file is attempted to load</exception>
        public SpriteComp(string fileName)
        {
            if (fileName.Contains("."))
            {
                Texture = LoadBitmap(fileName, TTengineMaster.ActiveGame.Content.RootDirectory, true);
            }
            this.fileName = fileName;
            InitTextures();
        }

        /// <summary>
        /// create new spritelet with given Texture2D texture, or null if no texture yet
        /// </summary>
        public SpriteComp(Texture2D texture)
        {
            this.texture = texture;
            InitTextures();
        }

        #endregion

        
        #region Class-internal properties

        protected Screenlet Screen = null;
        protected string fileName = null;
        protected float width = 0f;
        protected float height = 0f;
        private Texture2D texture = null;
        public static BlendState blendAlpha = null, blendColor = null;

        #endregion


        #region Properties
        
        /// <summary>
        /// width of sprite in normalized coordinates
        /// </summary>
        public float Width { get { return width; } set { width = value; } }

        /// <summary>
        /// height of sprite in normalized coordinates
        /// </summary>
        public float Height { get { return height; } set { height = value; } }

        /// <summary>
        /// Center of sprite expressed in relative width/height coordinates, where 1.0 is full width or full height.
        /// By default the center of the sprite is chosen in the middle.
        /// </summary>
        public Vector2 Center = new Vector2(0.5f, 0.5f); 

        /// <summary>
        /// calculates a center coordinate for direct use in Draw() calls, expressed in pixels
        /// </summary>
        public Vector2 DrawCenter = Vector2.Zero;

        /**
         * get/set the Texture of this sprite
         */
        public Texture2D Texture
        {
            set
            {
                texture = value;
                if (texture != null)
                {
                    Height = ToNormalizedNS(texture.Height);
                    Width = ToNormalizedNS(texture.Width);
                }
            }
            get
            {
                return texture;
            }

        }

        #endregion

        protected virtual void InitTextures()
        {
            if (texture != null)
            {
                Height = ToNormalizedNS(texture.Height);
                Width = ToNormalizedNS(texture.Width);
            }
            if (fileName != null && texture == null) 
                LoadTexture(fileName);
        }


        /**
         * load  a (first-time, or new) texture for this shape)
         */
        protected void LoadTexture(string textureFilename)
        {
            Texture = TTengineMaster.ActiveGame.Content.Load<Texture2D>(textureFilename);
        }

        /*
        public override void OnDraw(ref DrawParams p)
        {
            if (texture != null && Parent.Visible && Parent.Active )
            {                
                Parent.DrawC.MySpriteBatch.Draw(texture, Parent.DrawC.DrawPosition, null, Parent.DrawC.DrawColor,
                       Parent.Motion.RotateAbs, DrawCenter, Parent.DrawC.DrawScale, SpriteEffects.None, Parent.DrawC.LayerDepth);
            }
        }
         * */

        /*
        /// run collision detection of this against all other relevant Spritelets
        internal void HandleCollisions(UpdateParams p)
        {
            if (!Parent.Active || !Parent.Visible) return;

            // phase 1: check which items collide with me and add to list
            List<Gamelet> collItems = new List<Gamelet>();
            foreach (Gamelet s in Screen.collisionObjects)
            {
                if (s.Active  && s != Parent &&
                    CollidesWith(s) && s.Sprite.CollidesWith(Parent)) // a collision is detected
                {
                    collItems.Add(s);
                }
            }

            // phase 2: process the colliding items, to see if they are newly colliding or not
            foreach (Gamelet s in collItems)
            {
                if (!lastCollisionsList.Contains(s)) // if was not previously colliding, in previous round...
                {
                    // ... notify the collision methods
                    Parent.OnCollision(s);
                    Parent.OnCollideEventNotification(s);
                }
            }

            // phase 3: store list of colliding items for next round
            lastCollisionsList = collItems;
        }
         */

        internal float ToNormalizedNS(float coord)
        {
            return coord * Screen.scalingToNormalized;
        }

        /// <summary>
        /// load a bitmap direct from graphics file (ie bypass the XNA Content preprocessing framework).
        /// Method may be different whether it's at Initialize time or during game.
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="contentDir"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">when invalid image file is found</exception>
        protected Texture2D LoadBitmap(string fn, string contentDir, bool atRunTime)
        {
            // load texture
            try
            {
                if (atRunTime)
                    return LoadTextureStreamAtRuntime(Screen.graphicsDevice, fn, contentDir);
                else
                    return LoadTextureStream(Screen.graphicsDevice, fn, contentDir);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="fileName"></param>
        /// <param name="contentDir"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">when invalid image file is found</exception>
        private static Texture2D LoadTextureStreamAtRuntime(GraphicsDevice graphics, string fileName, string contentDir)
        {
            Texture2D tex = null;
            //RenderTarget2D result = null;

            using (Stream titleStream = File.Open(Path.Combine(contentDir, fileName), FileMode.Open)) //TitleContainer.OpenStream
            {
                try
                {
                    tex = Texture2D.FromStream(graphics, titleStream);
                }
                catch (InvalidOperationException ex)                
                {
                    // invalid or corrupt image file was loaded
                    throw(ex);
                }
            }
            // in case of png file, apply pre-multiply on texture color data:
            // http://blogs.msdn.com/b/shawnhar/archive/2009/11/10/premultiplied-alpha-in-xna-game-studio.aspx
            // http://blogs.msdn.com/b/shawnhar/archive/2010/04/08/premultiplied-alpha-in-xna-game-studio-4-0.aspx
            if (fileName.ToLower().EndsWith(".png") || fileName.ToLower().EndsWith(".dat"))
            {
                Color[] data = new Color[tex.Width * tex.Height];
                tex.GetData<Color>(data);
                for (long i = data.LongLength - 1; i != 0; --i)
                {
                    data[i] = Color.FromNonPremultiplied(data[i].ToVector4());
                }
                tex.SetData<Color>(data);
            }

            return tex ;
        }

        private static Texture2D LoadTextureStream(GraphicsDevice graphics, string loc, string contentDir)
        {
            Texture2D file = null;
            RenderTarget2D result = null;

            using (Stream titleStream = File.Open(Path.Combine(contentDir, loc), FileMode.Open)) //TitleContainer.OpenStream
            {                
                file = Texture2D.FromStream(graphics, titleStream);                
            }
            
            //Setup a render target to hold our final texture which will have premulitplied alpha values
            result = new RenderTarget2D(graphics, file.Width, file.Height);

            lock (graphics)
            {
                graphics.SetRenderTarget(result);
                graphics.Clear(Color.Black);

                //Multiply each color by the source alpha, and write in just the color values into the final texture
                if (blendColor == null)
                {
                    blendColor = new BlendState();
                    blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;
                    blendColor.AlphaDestinationBlend = Blend.Zero;
                    blendColor.ColorDestinationBlend = Blend.Zero;
                    blendColor.AlphaSourceBlend = Blend.SourceAlpha;
                    blendColor.ColorSourceBlend = Blend.SourceAlpha;
                }

                SpriteBatch spriteBatch = new SpriteBatch(graphics);
                spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
                spriteBatch.Draw(file, file.Bounds, Color.White);
                spriteBatch.End();

                //Now copy over the alpha values from the PNG source texture to the final one, without multiplying them
                if (blendAlpha == null)
                {
                    blendAlpha = new BlendState();
                    blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;
                    blendAlpha.AlphaDestinationBlend = Blend.Zero;
                    blendAlpha.ColorDestinationBlend = Blend.Zero;
                    blendAlpha.AlphaSourceBlend = Blend.One;
                    blendAlpha.ColorSourceBlend = Blend.One;
                }

                spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
                spriteBatch.Draw(file, file.Bounds, Color.White);
                spriteBatch.End();

                //Release the GPU back to drawing to the screen
                graphics.SetRenderTarget(null);
            }

            return result as Texture2D;
        }
    }
}
