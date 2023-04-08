using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace NFM_Core.Physics.Impulse;

public class AABB : Collider {
    public Rectangle Box => RigidBody.Transform.Bounds;
}
