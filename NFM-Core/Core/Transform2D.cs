using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace NFM_Core.Core;

public class Transform2D {
    public Vector2 Position {
        get => position;
        set {
            position = value;
            dirty = true;
        }
    }

    public Vector2 Scale {
        get => scale;
        set {
            scale = value;
            dirty = true;
        }
    }

    public Vector2 HalfScale => scale / 2;

    public float Rotation {
        get => rotation;
        set {
            rotation = value;
            dirty = true;
        }
    }

    public Rectangle Bounds {
        get {
            Recalculate();
            return bounds;
        }
    }

    private Vector2 position;
    private Vector2 scale;
    private float rotation;

    private Rectangle bounds;

    private bool dirty = false;

    private void Recalculate() {
        if (dirty) {
            bounds = new Rectangle((int)(position.X - HalfScale.X), (int)(position.Y - HalfScale.Y), (int)scale.X, (int)scale.Y);

            dirty = false;
        }
    }
}
