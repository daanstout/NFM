using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace NFM_Core.Physics.Impulse;

public class Solver {
    public void Check(RigidBody left, RigidBody right) {
        var manifold = new Manifold(left, right);

        if (left.Collider is AABB && right.Collider is AABB) {
            if (!AABBCollision((AABB)left.Collider, (AABB)right.Collider, manifold))
                return;
        } else if (left.Collider is Circle && right.Collider is Circle) {
            if (!CircleCollision((Circle)left.Collider, (Circle)right.Collider, manifold))
                return;
        } else if (left.Collider is AABB && right.Collider is Circle) {
            if (!AABBCircleCollision((AABB)left.Collider, (Circle)right.Collider, manifold))
                return;
        } else if (left.Collider is Circle && right.Collider is AABB) {
            if (!AABBCircleCollision((AABB)right.Collider, (Circle)left.Collider, manifold))
                return;
        } else
            throw new ArgumentException($"Cannot perform check between {left.Collider.GetType().FullName} and {right.Collider.GetType().FullName}");

        ResolveCollision(manifold);
        PositionalCorrection(manifold);
    }

    private static bool AABBCircleCollision(AABB left, Circle Right, Manifold manifold) {
        var direction = Right.Position - left.RigidBody.Transform.Position;

        var closest = direction;

        var xExtent = (left.Box.Right - left.Box.Left) / 2;
        var yExtent = (left.Box.Top - left.Box.Bottom) / 2;

        closest.X = Math.Clamp(-xExtent, xExtent, closest.X);
        closest.Y = Math.Clamp(-yExtent, yExtent, closest.Y);

        bool inside = false;

        if (direction == closest) {
            inside = true;

            if(MathF.Abs(direction.X) > MathF.Abs(direction.Y)) {
                if(closest.X > 0) {
                    closest.X = xExtent;
                } else {
                    closest.X = -xExtent;
                }
            } else {
                if (closest.Y > 0) {
                    closest.Y = yExtent;
                } else
                    closest.Y = -yExtent;
            }
        }

        var normal = direction - closest;
        var distance = normal.LengthSquared();
        var radius = Right.Radius;

        if (distance > radius * radius && !inside)
            return false;

        if (inside)
            manifold.SetCollisionData(radius - distance, -normal);
        else
            manifold.SetCollisionData(radius - distance, normal);

        return true;
    }

    private static bool AABBCollision(AABB left, AABB right, Manifold manifold) {
        var direction = right.Box.Location - left.Box.Location;

        var leftExtent = (left.Box.Right - left.Box.Left) / 2;
        var rightExtent = (right.Box.Right - right.Box.Left) / 2;

        var xOverlap = leftExtent + rightExtent - MathF.Abs(direction.X);

        if (xOverlap > 0) {
            leftExtent = (left.Box.Top - left.Box.Bottom) / 2;
            rightExtent = (right.Box.Top - right.Box.Bottom) / 2;

            var yOverlap = leftExtent + rightExtent - MathF.Abs(direction.Y);

            if (yOverlap > 0) {
                if (xOverlap > yOverlap) {
                    if (direction.X < 0) {
                        manifold.SetCollisionData(xOverlap, new Vector2(-1, 0));
                    } else {
                        manifold.SetCollisionData(xOverlap, new Vector2(1, 0));
                    }
                } else {
                    if (direction.Y < 0) {
                        manifold.SetCollisionData(yOverlap, new Vector2(0, -1));
                    } else {
                        manifold.SetCollisionData(yOverlap, new Vector2(0, 1));
                    }
                }
                return true;
            }
        }

        return false;
    }

    private static bool CircleCollision(Circle left, Circle right, Manifold manifold) {
        var distanceVec = left.Position - right.Position;
        var size = left.Radius + right.Radius;
        size *= size;

        if (distanceVec.LengthSquared() > size)
            return false;

        var distance = distanceVec.Length();

        if (distance != 0.0f) {
            manifold.SetCollisionData(size - distance, distanceVec / distance);
        } else {
            manifold.SetCollisionData(left.Radius, Vector2.UnitY);
        }

        return true;
    }

    private static void ResolveCollision(Manifold manifold) {
        var left = manifold.Left;
        var right = manifold.Right;

        var relativeVelocity = right.Velocity - left.Velocity;
        var collisionNormal = right.Transform.Position - left.Transform.Position;

        var velocityAlongNormal = Vector2.Dot(relativeVelocity, collisionNormal);

        if (velocityAlongNormal > 0)
            return;

        var restitution = MathF.Min(left.Restitution, right.Restitution);

        var j = -(1 + restitution) * velocityAlongNormal;
        j /= (left.InverseMass + right.InverseMass);

        var impulse = j * collisionNormal;
        left.Velocity -= left.InverseMass * impulse;
        right.Velocity += right.InverseMass * impulse;
    }

    private static void PositionalCorrection(Manifold manifold) {
        var percent = 0.2f;
        var slop = 0.01f;
        Vector2 correction = MathF.Max(manifold.Penetration - slop, 0.0f) / (manifold.Left.InverseMass + manifold.Right.InverseMass) * percent * manifold.Normal;
        manifold.Left.Transform.Position -= manifold.Left.InverseMass * correction;
        manifold.Right.Transform.Position += manifold.Right.InverseMass * correction;
    }
}
