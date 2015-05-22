// (c) 2010-2015 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;

namespace TTengine.Systems
{
    /// <summary>
    /// Defines the order in which all TTengine Systems are executed
    /// </summary>
    public class SystemsSchedule
    {
        // Systems in UPDATE loop
        public const int
            ScriptUpdateSystem  = 1,
            BTAISystem          = 1,
            MoveSystem          = 2,
            TargetMoveSystem    = 2,
            RotateSystem        = 2,
            ScaleSystem         = 2,
            MoveAbsSystem       = 3,
            ScaleToDrawscaleSystem = 3,
            RotateToDrawrotateSystem = 3,
            BlinkSystem         = 3,
            ExpirationSystem    = 3,
            CollisionSystem     = 4;

        // Systems in DRAW loop
        public const int 
            ScreenletPreSystem  = 0,
            AudioSystem         = 1,
            ScriptDrawSystem    = 2,
            SpriteRenderSystem  = 2,
            AnimatedSpriteSystem = 2,
            TextRenderSystem    = 2,
            ScreenletSystem     = 3;

    }
}
