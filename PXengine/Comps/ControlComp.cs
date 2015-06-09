
using Microsoft.Xna.Framework;

using Artemis.Interface;

namespace PXengine.Comps
{
    public class ControlComp: IComponent
    {
        public bool IsSteering = false;

        public Vector2 Move = Vector2.Zero;

        public double TimeBetweenMoves = 0.2;

        public double TimeBeforeNextMove = 0;

        public double PushForce = 1;

        public Vector2 PushFromOthers = Vector2.Zero;

    }
}
