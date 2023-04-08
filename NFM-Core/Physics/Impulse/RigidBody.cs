using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using NFM_Core.Components;

namespace NFM_Core.Physics.Impulse;

public class RigidBody : Component {
    public Collider Collider { get; set; }

    public Vector2 Velocity { get; set; }

    public Vector2 Acceleration { get; set; }

    /// <summary>
    /// Elasticity/Bounciness
    /// </summary>
    public float Restitution { get; set; }

    public float Mass {
        get => mass;
        set {
            mass = value;
            if (mass == 0) {
                inverseMass = 0.0f;
            } else {
                inverseMass = 1 / mass;
            }
        }
    }

    public float InverseMass => inverseMass;

    private float mass;
    private float inverseMass;

    public RigidBody() {
        Collider = new Circle();
    }

    public T SetCollider<T>() where T : Collider, new() {
        Collider = new T {
            RigidBody = this
        };

        return (T)Collider;
    }
}
