using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NFM_Core.Components;
using NFM_Core.Physics.Verlet;

namespace NFM_Core.Core;

public class Scene {
    internal Solver PhysicsSolver => world.PhysicsSolver;

    private readonly List<GameObject> gameObjects;
    private readonly List<Component> components;
    private SpriteBatch spriteBatch = default!;
    private readonly ContentManager contentManager;

    private readonly World world;

    public Scene(World world) {
        this.world = world;
        gameObjects = new();
        components = new();
        contentManager = new ContentManager(world.Services) {
            RootDirectory = "Content"
        };
    }

    public GameObject AddGameObject() {
        var gameObject = new GameObject(this);

        gameObjects.Add(gameObject);

        return gameObject;
    }
    
    internal void LoadContent() {
        spriteBatch = new SpriteBatch(world.GraphicsDevice);

        for (int i = 0; i < components.Count; i++) {
            components[i].LoadContent(contentManager);
        }
    }

    internal void FixedUpdate() {
        for (int i = 0; i < components.Count; i++) {
            components[i].FixedUpdate();
        }
    }

    internal void Update() {
        for(int i = 0; i < components.Count; i++) {
            if (components[i].FirstUpdate) {
                components[i].Start();
                components[i].FirstUpdate = false;
            }
            components[i].Update();
        }
    }

    internal void Render() {
        spriteBatch.Begin();

        for(int i = 0; i < components.Count; i++) {
            components[i].Render(spriteBatch);
        }

        spriteBatch.End();
    }

    internal T AddComponent<T>(Guid parent) where T : Component, new() {
        var component = new T {
            Parent = parent,
            Scene = this
        };

        component.LoadContent(contentManager);

        components.Add(component);

        return component;
    }

    internal T? GetComponent<T>(Guid parent) where T : Component {
        for(int i = 0; i < components.Count; i++) {
            if (components[i].Parent == parent && components[i] is T casted) {
                return casted;
            }
        }

        return null;
    }

    internal GameObject? GetGameObject(Guid id) {
        for(int i = 0; i < gameObjects.Count; i++) {
            if (gameObjects[i].ID == id) {
                return gameObjects[i];
            }
        }
        return null;
    }
}
