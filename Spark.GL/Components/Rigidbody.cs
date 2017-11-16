using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.GL
{
    public class Rigidbody : Component
    {
        internal Vec3 velocity = new Vec3();
        internal bool onG = false;
        internal void RBUpdate()
        {
            velocity.Y -= 0.01f;
        }
        internal void RBUpdate2()
        {
            gameObject.transform.Position.Y += velocity.Y;
        }
    }
}
