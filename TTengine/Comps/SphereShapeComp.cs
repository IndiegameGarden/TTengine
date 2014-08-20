using System;
using System.Collections.Generic;
using Artemis;
using Artemis.Interface;

namespace TTengine.Comps
{
    /// <summary>
    /// Entities that possess a Sphere shape, or a spherical (3D) collission region
    /// </summary>
    public class SphereShapeComp: IComponent
    {
        public float Radius { get; set; }

        /// <summary>
        /// List of Entities that is currently colliding with this Entity(shape)
        /// </summary>
        public List<Entity> Colliders = new List<Entity>();

        public SphereShapeComp(float radius)
        {
            this.Radius=radius;
        }

    }
}
