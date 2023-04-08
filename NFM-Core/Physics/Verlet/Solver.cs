using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using NFM_Core.Components;
using NFM_Core.Core;
using NFM_Core.Physics.SpatialPartitioning;

namespace NFM_Core.Physics.Verlet;

public class Solver {
    private const int STEPS_PER_UPDATE = 4;

    public Vector2 Gravity { get; set; } = new Vector2(0.0f, 1000.0f);

    public Vector2 ConstraintCenter { get; set; } = new Vector2(800, 450);

    public float ConstraintRadius { get; set; } = 400;
    public Vector2 ConstraintSize { get; set; } = new Vector2(400, 400);

    private readonly List<PhysicsBody> bodies = new();
    private readonly ISpatialPartitioner partitions;

    internal Solver(ISpatialPartitioner partitions) {
        this.partitions = partitions;
    }

    internal void AddPhysicsBody(PhysicsBody body) {
        bodies.Add(body);
        partitions.AddBody(body);
    }

    internal void RemovePhysicsBody(PhysicsBody body) {
        bodies.Remove(body);
        partitions.RemoveBody(body);
    }

    internal void Step() {
        var stepTime = (float)Time.FixedDeltaTimeSpan.TotalSeconds / 8.0f;

        for (int i = 0; i < STEPS_PER_UPDATE; i++) {
            ApplyGravity();
            CheckCollisions();
            ApplyConstraint();
            UpdateObjects(stepTime);
        }
    }

    private void ApplyGravity() {
        for (int i = 0; i < bodies.Count; i++) {
            bodies[i].Acceleration += Gravity;
        }
    }

    private void CheckCollisions() {
        const float RESPONSE_COEFFICIENT = 0.1f;

        for (int i = 0; i < bodies.Count; i++) {
            var current = bodies[i];
            foreach (var compare in partitions.GetBodiesNearBody(current)) {
                var distanceVec = current.Transform.Position - compare.Transform.Position;
                var distance = distanceVec.X * distanceVec.X + distanceVec.Y * distanceVec.Y;
                var minimumDistance = current.Transform.HalfScale.X + compare.Transform.HalfScale.X;

                if (distance < minimumDistance * minimumDistance) {
                    var distanceSqrt = MathF.Sqrt(distance);
                    var overlap = distanceVec / distanceSqrt;
                    var currentMassRatio = current.Mass / (current.Mass + compare.Mass);
                    var compareMassRatio = 1 - currentMassRatio;
                    var dt = 0.5f * RESPONSE_COEFFICIENT * (distanceSqrt - minimumDistance);

                    current.Transform.Position -= overlap * (compareMassRatio * dt);
                    compare.Transform.Position += overlap * (currentMassRatio * dt);
                }
            }
            //for (int j = i + 1; j < bodies.Count; j++) {
            //    var compare = bodies[j];

            //    var distanceVec = current.Transform.Position - compare.Transform.Position;
            //    var distance = distanceVec.X * distanceVec.X + distanceVec.Y * distanceVec.Y;
            //    var minimumDistance = current.Transform.HalfScale.X + compare.Transform.HalfScale.X;

            //    if (distance < minimumDistance * minimumDistance) {
            //        var distanceSqrt = MathF.Sqrt(distance);
            //        var overlap = distanceVec / distanceSqrt;
            //        var currentMassRatio = current.Mass / (current.Mass + compare.Mass);
            //        var compareMassRatio = 1 - currentMassRatio;
            //        var dt = 0.5f * RESPONSE_COEFFICIENT * (distanceSqrt - minimumDistance);

            //        current.Transform.Position -= overlap * (compareMassRatio * dt);
            //        compare.Transform.Position += overlap * (currentMassRatio * dt);
            //    }
            //}
        }
    }

    private void ApplyConstraint() {
        var leftBounds = ConstraintCenter.X - (ConstraintSize.X / 2);
        var rightBounds = ConstraintCenter.X + (ConstraintSize.X / 2);
        var upperBounds = ConstraintCenter.Y - (ConstraintSize.Y / 2);
        var lowerBounds = ConstraintCenter.Y + (ConstraintSize.Y / 2);

        for (int i = 0; i < bodies.Count; i++) {
            if (bodies[i].Transform.Position.X + bodies[i].Transform.HalfScale.X > rightBounds)
                bodies[i].Transform.Position = new Vector2(rightBounds - bodies[i].Transform.HalfScale.X, bodies[i].Transform.Position.Y);
            else if (bodies[i].Transform.Position.X - bodies[i].Transform.HalfScale.X < leftBounds)
                bodies[i].Transform.Position = new Vector2(leftBounds + bodies[i].Transform.HalfScale.X, bodies[i].Transform.Position.Y);

            if (bodies[i].Transform.Position.Y + bodies[i].Transform.HalfScale.Y > lowerBounds)
                bodies[i].Transform.Position = new Vector2(bodies[i].Transform.Position.X, lowerBounds - bodies[i].Transform.HalfScale.Y);
            else if (bodies[i].Transform.Position.Y - bodies[i].Transform.HalfScale.Y < upperBounds)
                bodies[i].Transform.Position = new Vector2(bodies[i].Transform.Position.X, upperBounds + bodies[i].Transform.HalfScale.Y);

            //var offset = ConstraintCenter - bodies[i].Transform.Position;
            //var distance = MathF.Sqrt(offset.X * offset.X + offset.Y * offset.Y);

            //if (distance > ConstraintRadius - bodies[i].Transform.HalfScale.X) {
            //    var toMove = offset / distance;
            //    bodies[i].Transform.Position = ConstraintCenter - toMove * (ConstraintRadius - bodies[i].Transform.HalfScale.X);
            //}
        }
    }

    private void UpdateObjects(float delta) {
        for (int i = 0; i < bodies.Count; i++) {
            bodies[i].Update(delta);
            partitions.UpdateBodyCell(bodies[i]);
        }
    }
}
