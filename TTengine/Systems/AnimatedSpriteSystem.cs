
namespace TTengine.Systems
{
    #region Using statements

    using System;

    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using TTengine.Core;
    using TTengine.Comps;

    #endregion

    /// <summary>The system for rendering animated sprites</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.AnimatedSpriteSystemDraw)]
    public class AnimatedSpriteSystem : EntityComponentProcessingSystem<AnimatedSpriteComp, PositionComp, DrawComp>
    {

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(Entity entity, AnimatedSpriteComp spriteComp, PositionComp posComp, DrawComp drawComp)
        {
            if (!drawComp.IsVisible)
                return;

            var scr = drawComp.DrawScreen;

            // update drawpos
            var p = posComp.PositionAbs;
            drawComp.DrawPosition = scr.ToPixels(p);
            drawComp.LayerDepth = posComp.Depth; 

            spriteComp.frameSkipCounter--;
            if (spriteComp.frameSkipCounter == 0)
            {
                spriteComp.frameSkipCounter = spriteComp.SlowdownFactor;

                // update frame counter - one per frame
                switch (spriteComp.AnimType)
                {
                    case AnimationType.NORMAL:
                        spriteComp.CurrentFrame++;
                        if (spriteComp.CurrentFrame > spriteComp.MaxFrame || spriteComp.CurrentFrame == spriteComp.TotalFrames)
                            spriteComp.CurrentFrame = spriteComp.MinFrame;
                        break;

                    case AnimationType.REVERSE:
                        spriteComp.CurrentFrame--;
                        if (spriteComp.CurrentFrame < spriteComp.MinFrame || spriteComp.CurrentFrame < 0)
                            spriteComp.CurrentFrame = spriteComp.MaxFrame;
                        break;

                    case AnimationType.PINGPONG:
                        spriteComp.CurrentFrame += spriteComp.pingpongDelta;
                        if (spriteComp.CurrentFrame > spriteComp.MaxFrame || spriteComp.CurrentFrame == spriteComp.TotalFrames)
                        {
                            spriteComp.CurrentFrame -= 2;
                            spriteComp.pingpongDelta = -spriteComp.pingpongDelta;
                        }
                        else if (spriteComp.CurrentFrame < spriteComp.MinFrame || spriteComp.CurrentFrame < 0)
                        {
                            spriteComp.CurrentFrame += 2;
                            spriteComp.pingpongDelta = -spriteComp.pingpongDelta;
                        }
                        break;
                }
            }

            // draw sprite from sprite atlas
            TTSpriteBatch sb = scr.SpriteBatch;
            int row = (int)((float)spriteComp.CurrentFrame / (float)spriteComp.Nx);
            int column = spriteComp.CurrentFrame % spriteComp.Nx;
            Vector2 dp = drawComp.DrawPosition;
            Rectangle sourceRectangle = new Rectangle(spriteComp.px * column, spriteComp.py * row, spriteComp.px, spriteComp.py);

            sb.Draw(spriteComp.Texture, dp, sourceRectangle, drawComp.DrawColor,
                drawComp.DrawRotation, spriteComp.Center, drawComp.DrawScale, SpriteEffects.None, drawComp.LayerDepth);

        }

    }
}