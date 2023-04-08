
using Microsoft.Xna.Framework;

using NFM_Core.Components;
using NFM_Core.Core;
using NFM_Core.Physics.Verlet;

using var game = new World();

var obj = game.Scene.AddGameObject();
obj.AddComponent<BallSpawner>();
//obj.Transform.Position = new Vector2(800, 450);
//obj.Transform.Scale = new Vector2(50, 50);
//obj.AddComponent<PhysicsBody>();
//obj.AddComponent<SpriteRenderer>();

//var obj2 = game.Scene.AddGameObject();
//obj2.Transform.Position = new Vector2(825, 575);
//obj2.Transform.Scale = new Vector2(50, 50);
//obj2.AddComponent<PhysicsBody>();
//obj2.AddComponent<SpriteRenderer>();


game.Run();
