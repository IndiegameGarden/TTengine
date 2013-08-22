using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTengine.Comps
{
    public class ShapeComp: IComponent
    {

        /// <summary>Initializes a new instance of the <see cref="VelocityComp" /> class.</summary>
        public ShapeComp()
            : this(0.0f, 0.0f)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="VelocityComp" /> class.</summary>
        /// <param name="velocity">The velocity.</param>
        public VelocityComp(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Gets or sets the position.</summary>
        /// <value>The position.</value>
        public Vector2 Velocity
        {
            get
            {
                return new Vector2(this.X, this.Y);
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }


        /*
        /// <summary>Initializes a new instance of the <see cref="VelocityComp" /> class.</summary>
        /// <param name="velocity">The velocity.</param>
        /// <param name="angle">The angle.</param>
        public VelocityComp(float velocity, float angle)
        {
            this.Speed = velocity;
            this.Angle = angle;
        }
         */

        public float X { get; set; }
        public float Y { get; set; }

        /*
        /// <summary>Gets or sets the angle.</summary>
        /// <value>The angle.</value>
        public float Angle { get; set; }

        /// <summary>Gets the angle as radians.</summary>
        /// <value>The angle as radians.</value>
        public float AngleAsRadians
        {
            get
            {
                return this.Angle * ToRadians;
            }
        }
        */

        /// <summary>Gets or sets the speed.</summary>
        /// <value>The speed.</value>
        public float Speed
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y);
            }
            set
            {
                // TODO
            }
        }

        /*
        /// <summary>The add angle.</summary>
        /// <param name="angle">The angle.</param>
        public void AddAngle(float angle)
        {
            this.Angle = (this.Angle + angle) % 360;
        }
         */
    }
}
    }
}
