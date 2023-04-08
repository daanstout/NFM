using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NFM_Core.Physics.SpatialPartitioning;
using NFM_Core.Physics.Verlet;

namespace NFM_Core.Core;

public class World : Game {
    public static List<string> TitleAdditions { get; set; } = new List<string>();
    
    public Scene Scene { get; }

    public Solver PhysicsSolver { get; }

    private readonly GraphicsDeviceManager graphics;

    private TimeSpan timeTillNextPhysicsUpdate = TimeSpan.Zero;
    private readonly TimeSpan timeBetweenPhysicsUpdates = TimeSpan.FromMilliseconds(1000 / 60);

    public World() {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1600;
        graphics.PreferredBackBufferHeight = 900;
        graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        PhysicsSolver = new(new FixedGrid(10, 10, 160, 90, Vector2.Zero));
        Scene = new(this);
    }

    protected override void Initialize() {
        // TODO: Add your initialization logic here
        
        base.Initialize();
    }

    protected override void LoadContent() {
        Scene.LoadContent();
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Time.Update(gameTime);

        timeTillNextPhysicsUpdate -= gameTime.ElapsedGameTime;

        if (timeTillNextPhysicsUpdate.TotalMilliseconds <= 0.0f) {
            Time.FixedUpdate(timeBetweenPhysicsUpdates);
            timeTillNextPhysicsUpdate += timeBetweenPhysicsUpdates;
            var sw = Stopwatch.StartNew();
            PhysicsSolver.Step();
            sw.Stop();
            Window.Title = sw.Elapsed.TotalMilliseconds.ToString() + " " + string.Join("", TitleAdditions);
            Scene.FixedUpdate();
        }

        Scene.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Scene.Render();

        base.Draw(gameTime);
    }
}
