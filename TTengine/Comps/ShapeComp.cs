using System;
using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Entities that possess a Sphere shape, or a spherical (3D) collission region
    /// </summary>
    public class ShapeComp: IComponent
    {
        public const string CollisionGroupName = "TTsphere";
    
        public float Radius { get; set; }

        public ShapeComp(float radius)
        {
            this.Radius=radius;
        }

    }
}
