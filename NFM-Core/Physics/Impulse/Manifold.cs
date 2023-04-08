using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace NFM_Core.Physics.Impulse;

public class Manifold {
    public RigidBody Left { get; init; }

    public RigidBody Right { get; init; }

    public float Penetration { get; private set; }

    public Vector2 Normal { get; private set; }

    public Manifold(RigidBody left, RigidBody right) {
        Left = left;
        Right = right;
    }

    public void SetCollisionData(float penetration, Vector2 normal) {
        Penetration = penetration;
        Normal = normal;
    }
}
