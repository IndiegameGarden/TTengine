using System;
using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Entities that possess a Sphere shape, or a spherical (3D) collission region
    /// FIXME rename to SphereShapeComp
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
