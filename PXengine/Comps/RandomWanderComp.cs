
using System;
using Microsoft.Xna.Framework;

using Artemis.Interface;

namespace PXengine.Comps
{
    public class RandomWanderComp: IComponent
    {
        public RandomWanderComp()
        {
        }

        /// <summary>current random direction of wandering - may be changed by an external object</summary>
        public Vector2 CurrentDirection = Vector2.Zero;

        /// <summary>The max for the random time it takes to change direction</summary>
        public double MaxDirectionChangeTime = 10;

        /// <summary>The min for the random time it takes to change direction</summary>
        public double MinDirectionChangeTime = 2;

        /// <summary>The remaining time it will take, counted from this moment on, before doing a new change of direction</summary>
        public double CurrentDirectionChangeTime = 0;


    }
}
