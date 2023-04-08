using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NFM_Core.Components;

public class SpriteRenderer : Component {
    private Texture2D texture = default!;

    public override void LoadContent(ContentManager contentManager) {
        texture = contentManager.Load<Texture2D>("ball");
    }

    public override void Render(SpriteBatch spriteBatch) {
        var rect = new Rectangle {
            X = (int)(Transform.Position.X - Transform.HalfScale.X),
            Y = (int)(Transform.Position.Y - Transform.HalfScale.Y),
            Width = (int)Transform.Scale.X,
            Height = (int)Transform.Scale.Y
        };

        spriteBatch.Draw(texture, rect, null, Color.White, Transform.Rotation, Vector2.One / 2, SpriteEffects.None, 0.0f);
    }
}
