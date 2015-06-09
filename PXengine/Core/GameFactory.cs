using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Artemis;
using Artemis.Interface;
using TreeSharp;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Behaviors;
using TTengine.Modifiers;
using TTengine.Util;

using PXengine.Comps;

namespace PXengine.Core
{
    /// <summary>
    /// Factory to create new game-specific entities
    /// </summary>
    public class GameFactory
    {
        private GameFactory(PXGame game)
        {
            _game = game;
        }

        private static GameFactory _instance = null;
        private PXGame _game;
        protected Random rnd = new Random();

        public static GameFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameFactory(TTGame.Instance as PXGame);
                return _instance as GameFactory;
            }
        }

        public Entity CreatePixie(Color color)
        {
            Entity e = CreateThing(ThingType.HERO, true, color);
            e.AddComponent(new UserControlComp());
            return e;
        }

        public Entity CreateThing(ThingType tp, bool hasControls, string bitmap, Color color)
        {
            var e = TTFactory.CreateSpritelet(bitmap);
            var sc = e.GetComponent<SpriteComp>();
            var tc = new ThingComp(tp);
            var dc = e.GetComponent<DrawComp>();
            dc.DrawColor = color;
            e.AddComponent(tc);
            e.AddComponent(new TargetMotionComp());
            if (hasControls)
            {
                var tcc = new ControlComp();
                e.AddComponent(tcc);
            }
            tc.PassableIntensityThreshold = PXGame.Instance.Level.DefaultPassableIntensityThreshold;

            return e;
        }

        public Entity CreateThing(ThingType tp, bool hasControls, Color color)
        {
            return CreateThing(tp, hasControls, "pixie", color);
        }

        public ColorCycleComp CreateColorCycling(float cyclePeriod, Color minColor, Color maxColor)
        {
            ColorCycleComp cycl = new ColorCycleComp(cyclePeriod);
            cycl.minColor = minColor;
            cycl.maxColor = maxColor;
            return cycl;
        }

        public Entity CreateSubtitle(string text, Color color)
        {
            var e = CreateSubtitle(new SubtitleText(text));
            e.GetComponent<DrawComp>().DrawColor = color;
            return e;
        }

        public Entity CreateSubtitle(SubtitleText stComp)
        {
            var e = TTFactory.CreateDrawlet();
            e.AddComponent(stComp);
            e.Refresh();
            return e;
        }

        public Entity CreateLevelet(string bitmapFile)
        {
            var e = TTFactory.CreateSpritelet(bitmapFile);
            //e.AddComponent(new ScaleComp());
            e.Refresh();
            return e;
        }
        
    }
}
