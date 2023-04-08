using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace NFM_Core.Physics.Impulse;

public class Circle : Collider {
    public Vector2 Position => RigidBody.Transform.Position;

    public float Radius => RigidBody.Transform.HalfScale.X;
}
