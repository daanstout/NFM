using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using NFM_Core.Core;
using NFM_Core.Physics.Verlet;

namespace NFM_Core.Components;

public class BallSpawner : Component {
    private readonly TimeSpan timeBetweenSpawns = TimeSpan.FromMilliseconds(17);
    private readonly int maxSpawned = 40;
    private TimeSpan timeTillNextSpawn = TimeSpan.Zero;
    private int spawned = 0;
    private Random random = new();

    public override void Update() {
        timeTillNextSpawn -= Time.DeltaTimeSpan;

        if (timeTillNextSpawn.TotalSeconds <= 0.0f && spawned < maxSpawned) {
            spawned++;
            World.TitleAdditions = new List<string> {
                $"{spawned} balls"
            };

            var obj = Scene.AddGameObject();
            obj.Transform.Position = new Vector2(random.Next(775, 825), 450);
            var size = random.Next(5, 15);
            obj.Transform.Scale = new Vector2(size, size);
            obj.AddComponent<PhysicsBody>().Mass = size;
            obj.AddComponent<SpriteRenderer>();

            timeTillNextSpawn += timeBetweenSpawns;
        }
    }
}
