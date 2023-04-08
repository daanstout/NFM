using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using NFM_Core.Components;
using NFM_Core.Core;

namespace NFM_Core.Physics.Verlet;

public class PhysicsBody : Component {
    public Vector2 Acceleration { get; set; }

    public float Mass { get; set; } = 1;

    internal int CellX { get; set; }

    internal int CellY { get; set; }

    private Vector2 lastPosition { get; set; }

    public void Update(float delta) {
        var position = Transform.Position;
        var displacement = position - lastPosition;
        lastPosition = position;
        position += displacement + Acceleration * (float)(delta * delta);
        Acceleration = Vector2.Zero;
        Transform.Position = position;
    }

    public override void Start() {
        lastPosition = Transform.Position;
        Scene.PhysicsSolver.AddPhysicsBody(this);
    }

    public override void End() {
        Scene.PhysicsSolver.RemovePhysicsBody(this);
    }
}
