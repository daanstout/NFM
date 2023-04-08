using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using NFM_Core.Core;

namespace NFM_Core.Components;

public class Component : Core.Object {
    internal Guid Parent { get; init; }

    internal Scene Scene { get; init; } = default!;

    public GameObject GameObject
    {
        get
        {
            gameObject ??= Scene.GetGameObject(Parent);

            return gameObject ?? throw new InvalidOperationException("Missing Game Object!");
        }
    }

    public Transform2D Transform => GameObject.Transform;

    internal bool FirstUpdate { get; set; } = true;

    private GameObject? gameObject;

    public virtual void LoadContent(ContentManager contentManager) { }

    public virtual void Start() { }

    public virtual void FixedUpdate() { }

    public virtual void Update() { }

    public virtual void End() { }

    public virtual void Render(SpriteBatch spriteBatch) { }
}
