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

    /// <summary>The system for rendering text</summary>
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Draw, Layer = SystemsSchedule.TextRenderSystemDraw)]
    public class TextRenderSystem : EntityComponentProcessingSystem<TextComp, PositionComp, DrawComp>
    {
        public override void Process(Entity entity, TextComp textComp, PositionComp posComp, DrawComp drawComp)
        {
            if (!drawComp.IsVisible)
                return;
            
            var scr = drawComp.DrawScreen;

            // update drawpos FIXME - should one system do this, now it's two? or make a helper method.
            var p = posComp.PositionAbs;
            drawComp.DrawPosition = scr.ToPixels(p);
            drawComp.LayerDepth = posComp.Depth; 

            // draw sprite
            TTSpriteBatch sb = scr.SpriteBatch;
            sb.DrawString(textComp.Font, textComp.Text, drawComp.DrawPosition, drawComp.DrawColor, 0f, 
                Vector2.Zero, drawComp.DrawScale, SpriteEffects.None, drawComp.LayerDepth);

        }

    }
}