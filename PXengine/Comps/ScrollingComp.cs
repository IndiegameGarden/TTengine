
using Microsoft.Xna.Framework;

using Artemis.Interface;
using TTengine.Core;
using TTengine.Comps;

namespace PXengine.Comps
{
    /// <summary>
    /// Scrolls the ZoomCenter of Screen to track the Position of this Entity
    /// </summary>
    public class ScrollingComp: IComponent
    {
        public ScrollingComp(Vector2 initialPosition)
        {
            Scrolling = new TargetVector(initialPosition);
        }

        public TargetVector Scrolling;

    }
}
