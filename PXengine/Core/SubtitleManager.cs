using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TTengine.Core;
using TTengine.Comps;
using Artemis;

namespace PXengine.Core
{
    /**
     * Displays subtitle text, manages conflicts
     */
    public class SubtitleManager: IScript
    {
        /// <summary>
        /// Helper class to manage the queue of subtitle text items
        /// </summary>
        class QueuedText
        {
            public QueuedText(int priority, Entity text)
            {
                this.Priority = priority;
                this.Text = text;
            }

            public Entity Text;
            public int Priority = 0;
        }

        List<QueuedText> q = new List<QueuedText>();
        QueuedText currentItem = null;

        public SubtitleManager()
        {            
        }

        public void Show(int priority, SubtitleText st)
        {
            var e = GameFactory.Instance.CreateSubtitle(st);
            q.Add(new QueuedText(priority, e));
            e.IsEnabled = false;
        }

        public Entity Show(int priority, string txt, double duration, Color color)
        {
            var st = GameFactory.Instance.CreateSubtitle(txt, color);
            st.AddComponent(new ExpiresComp(duration));
            q.Add(new QueuedText(priority,st));
            st.IsEnabled = false;
            return st;
        }

        public Entity Show(int priority, string txt, double duration)
        {
            return Show(priority, txt, duration, Color.WhiteSmoke);
        }

        public void OnUpdate(ScriptContext p)
        {
            if (q.Count == 0)
                return;
           
            // clean up q - items that should be removed
            List<QueuedText> toRemove = new List<QueuedText>();
            foreach (QueuedText t in q)
            {
                if (!t.Text.IsActive)
                    toRemove.Add(t);
            }
            foreach (QueuedText t in toRemove)
            {
                q.Remove(t);
            }

            // check which item should be displayed
            // the highest priority one which is earliest in queue
            int highestPri = -32000;
            QueuedText winnerItem = null;
            foreach (QueuedText t in q)
            {
                // by default, text is not active. Only the winning text will be.
                t.Text.IsEnabled = false; 
                if (t.Priority > highestPri /*&& t.Text.StartTime <= SimTime */ )
                {
                    highestPri = t.Priority;
                    winnerItem = t;
                }
            }

            // activate only the winning item
            if (winnerItem != null )
            {
                currentItem = winnerItem;
                currentItem.Text.IsEnabled = true;
            }
            
        }

        public void OnDraw(ScriptContext ctx)
        {
        }

    }
}
