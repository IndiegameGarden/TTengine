// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

using TTengine.Core;

using Artemis;
using Artemis.Interface;

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
        /// <param name="screenComp">reference to the ScreenComp to which this sprite will be drawn,
        /// to learn information about screen size.</param>
        /// <exception cref="InvalidOperationException">when invalid image file is attempted to load</exception>
        public SpriteComp(string fileName)
        {
            if (fileName.Contains("."))
            {
                Texture = LoadBitmap(fileName, TTGame.Instance.Content.RootDirectory, true);
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

        protected string fileName = null;
        protected float width = 0f;
        protected float height = 0f;
        private Texture2D texture = null;
        public static BlendState blendAlpha = null, blendColor = null;

        #endregion


        #region Properties

        /// <summary>
        /// width of sprite in pixels
        /// </summary>
        public float Width { get { return width; } private set { width = value; } }

        /// <summary>
        /// height of sprite in pixels
        /// </summary>
        public float Height { get { return height; } private set { height = value; } }

        /// <summary>
        /// Center of sprite expressed in relative width/height coordinates, where 1.0 is full width or full height
        /// of the sprite. By default the center of the sprite is chosen in the middle.
        /// </summary>
        public Vector3 Center = new Vector3(0.5f, 0.5f, 0f); 

        /// <summary>
        /// based on Center this is a center coordinate for direct use in Draw() calls, expressed in pixels
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
                InitTextures();
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
                Height = texture.Height;
                Width = texture.Width;
                DrawCenter = new Vector2(Center.X * Width, Center.Y * Height); // FIXME adapt all the time (if center changes)??
            }
            if (fileName != null && texture == null)
                LoadTexture(fileName);
        }


        /**
         * load  a (first-time, or new) texture for this shape)
         */
        protected void LoadTexture(string textureFilename)
        {
            Texture = TTGame.Instance.Content.Load<Texture2D>(textureFilename);
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
                    return LoadTextureStreamAtRuntime( TTGame.Instance.GraphicsDevice, fn, contentDir);
                else
                    return LoadTextureStream(TTGame.Instance.GraphicsDevice, fn, contentDir);
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

                //Release the GPU back to drawing to the screenletEntity
                graphics.SetRenderTarget(null);
            }

            return result as Texture2D;
        }
    }
}
