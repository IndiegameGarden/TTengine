using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Systems
{
    /// <summary>
    /// Defines the order in which all TTengine Systems are executed
    /// </summary>
    public class SystemsSchedule
    {
        // Systems in UPDATE loop
        public const int
            MovementSystem  = 1,
            RotateSystem    = 1,
            ScaleSystem     = 1,
            ScriptSystemUpdate = 1,
            BTAISystem      = 2,
            BlinkSystem     = 3,
            ExpirationSystem = 3,
            CollisionSystem = 4;

        // Systems in DRAW loop
        public const int 
            ScreenletPreSystem = 0,
            AudioSystem     = 1,
            ScriptSystemDraw = 2,
            SpriteRenderSystem = 2,
            TextRenderSystem = 2,
            ScreenletSystem = 3;

    }
}
