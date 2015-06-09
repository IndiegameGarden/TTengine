// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTengine.Core;
using TTengine.Util;

namespace PXengine.Core
{
    /**
     * Displays a subtitle on screen for a specified time (no rotation or scale at this moment)
     * Text strings may contain Enter char (backslash n). It auto-positions.
     */
    public class SubtitleText : Comp
    {
        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// scaling vector for subtitles text (horizontal scale, vertical scale)
        /// </summary>
        public Vector2 ScaleVector = new Vector2(1.5f, 1.5f);

        /// <summary>
        /// displacement (in pixels) of the shadow below subtitles
        /// </summary>
        public Vector2 ShadowVector = new Vector2(2f, 2f);

        /// <summary>
        /// if true uses shadow effect for better visibility of subtitles
        /// </summary>
        public bool Shadow = true;

        public Color Color = Color.White;

        /// <summary>
        /// which font to use for drawing text (can be changed)
        /// </summary>
        public SpriteFont SubtitleFont;

        /// <summary>
        /// vertical spacing between lines for multi-line subtitles
        /// </summary>
        public float VerticalLineSpacing = 0.05f;

        // the default font - used if SubtitleFont not specified
        protected static SpriteFont DefaultSubtitleFont;

        public List<string> TextItems { get; private set; }
        public List<double> TextDurations { get; private set; }

        public double StartTime = 0;

        Vector2 totalTextSize;
        bool doReplace;
        //float nextTextStartTime = 0f;

        public SubtitleText()
        {
            TextItems = new List<string>();
            TextDurations = new List<double>();
        }

        /// <summary>
        /// create new SubtitleText from initial string
        /// </summary>
        /// <param name="initialText"></param>
        public SubtitleText( string initialText)
            :this()
        {
            TextItems.Add(initialText);
            TextDurations.Add(1);
            doReplace = true;
            InitFont();
        }

        /// <summary>
        /// create a new multitext, with multiple strings showing at indicated times.
        /// </summary>
        /// <param name="multiText"></param>
        /// <param name="timings">per multiText item, number of seconds that this item should display</param>
        /// <param name="doReplace">if true, only latest item from multiText is displayed. 
        ///                         If false, each item is added to the previous one.</param>
        public SubtitleText(string[] multiText, float[] timings, bool doReplace)
        {
            foreach (var t in multiText)
                TextItems.Add(t);
            foreach (var t in timings)
                TextDurations.Add(t);
            this.doReplace = doReplace;
            InitFont();
        }

        public void AddText(string text, double duration)
        {
            this.TextItems.Add(text);
            this.TextDurations.Add(duration);
        }

        private void InitFont()
        {
            if (DefaultSubtitleFont==null)
                DefaultSubtitleFont = TTGame.Instance.Content.Load<SpriteFont>("Subtitles1");
            SubtitleFont = DefaultSubtitleFont;
        }

        public string Text        
        {
            get
            {
                return TextItems[0];
            }
            set
            {
                TextItems[0] = value;
                AutoPosition();
            }
        }

        protected void AutoPosition()
        {
            int lines = TTUtil.LineCount(TextItems[0]);
            float yOffset = -((float)(lines - 1)) * VerticalLineSpacing - VerticalLineSpacing;

            Position = new Vector2(TTFactory.BuildChannel.Screen.Center.X, 1f+yOffset); // TODO move up for long texts
            // Use entire text size for calculating middle offsets 
            totalTextSize = SubtitleFont.MeasureString(TextItems[0]);
        }

        /*
        protected override void OnDraw(ref DrawParams p)
        {
            Vector2 pos = DrawInfo.DrawPosition;
            String curText = "";
            if (text.Length > 0 && text[0].Length > 0)
            {
                float t = 0f;
                for (int i = 0; i < text.Length; i++ )
                {
                    if (t <= SimTime)
                    {
                        if (doReplace)
                            curText = text[i];
                        else
                        {
                            if (curText.Length > 0)
                                curText += "\n";
                            curText += text[i];
                        }
                    }
                    t += timings[i];
                }
            }

            if (curText.Length > 0)
            {
                float sc = Motion.ScaleAbs;
                Vector2 origin = totalTextSize / 2f; // Vector2.Zero; // new Vector2(((float)curText.Length) / 40f, 0f);
                if (Shadow)
                {
                    MySpriteBatch.DrawString(SubtitleFont, curText, pos + ShadowVector, Color.Black, 0f, origin, ScaleVector * sc, SpriteEffects.None, DrawInfo.LayerDepth + 0.0001f);
                    MySpriteBatch.DrawString(SubtitleFont, curText, pos - ShadowVector, Color.DarkGray, 0f, origin, ScaleVector * sc, SpriteEffects.None, DrawInfo.LayerDepth + 0.0002f);
                }
                MySpriteBatch.DrawString(SubtitleFont, curText, pos, DrawInfo.DrawColor, 0f, origin, ScaleVector * sc, SpriteEffects.None, DrawInfo.LayerDepth);
            }
        }
         */
    }
}
