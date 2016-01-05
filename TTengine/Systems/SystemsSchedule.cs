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
            WorldSystem         = 0,
            ScriptSystem        = 1,
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
            WorldSystemDraw         = 0,        // world drawing goes depth-first.
            ScreenletPreSystemDraw  = 1,
            AudioSystemDraw         = 1,
            ScriptSystemDraw        = 2,
            SpriteRenderSystemDraw  = 2,
            AnimatedSpriteSystemDraw = 2,
            TextRenderSystemDraw    = 2,
            ScreenletPostSystemDraw = 3;

    }
}
