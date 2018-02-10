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
            WorldSystem         = 0,        // world simulation goes depth-first.
            ScriptSystem        = 1,
            BTAISystem          = 1,
            MoveSystem          = 2,
            TargetMoveSystem    = 2,
            RotateSystem        = 2,
            ScaleSystem         = 2,
            GeomSystem          = 2,
            MoveAbsSystem       = 3,
            ScaleToDrawscaleSystem = 3,
            RotateToDrawrotateSystem = 3,
            BlinkSystem         = 3,
            ExpirationSystem    = 3,
            CollisionSystem     = 4,
            BuilderSystem       = 5;

        // Systems in DRAW loop
        public const int
            WorldSystemDraw         = 0,        // world drawing goes depth-first.
            ScreenPreSystemDraw     = 1,
            AudioSystemDraw         = 1,
            ScriptSystemDraw        = 2,
            SpriteRenderSystemDraw  = 2,
            GeomSystemDraw          = 4,
            AnimatedSpriteSystemDraw = 2,
            TextRenderSystemDraw    = 2,
            ScreenPostSystemDraw    = 3;

    }
}
