using System;
using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Entities that possess a Sphere shape, or a spherical (3D) collission region
    /// </summary>
    public class SphereShapeComp: IComponent
    {
        public const string CollisionGroupName = "TTsphere";
    
        public float Radius { get; set; }

        public SphereShapeComp(float radius)
        {
            this.Radius=radius;
        }

    }
}
