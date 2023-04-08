using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFM_Core.Components;

namespace NFM_Core.Core;

public class GameObject : Object {
    public Transform2D Transform { get; }

    private Scene Scene { get; }

    internal GameObject(Scene scene) {
        Transform = new Transform2D();

        Scene = scene;
    }

    public T AddComponent<T>() where T : Component, new() {
        return Scene.AddComponent<T>(ID);
    }

    public T? GetComponent<T>() where T : Component {
        return Scene.GetComponent<T>(ID);
    }
}
