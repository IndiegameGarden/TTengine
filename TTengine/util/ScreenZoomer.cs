using Microsoft.Xna.Framework.Input;
using TTengine.Core;

namespace TTengine.Util
{
    /// <summary>
    /// Attach this to a Screenlet to control the screen's zooming using page-up / page-down keys
    /// </summary>
    public class ScreenZoomer: Gamelet
    {
        Screenlet screen = null;

        public override void OnNewParent(TTObject oldParent)
        {
            base.OnNewParent(oldParent);
            if (Parent is Screenlet)
            {
                screen = Parent as Screenlet;
            }
        }

        public override void OnDraw(ref DrawParams p)
        {
            base.OnDraw(ref p);

            if (screen != null)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
                {
                    screen.Motion.Zoom += 0.003f;
                    screen.DebugText(0.1f, 0.3f, "Zoom=" + screen.Motion.Zoom);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
                {
                    screen.Motion.Zoom -= 0.003f;
                    screen.DebugText(0.1f, 0.3f, "Zoom=" + screen.Motion.Zoom);
                }
            }

        }

    }
}
