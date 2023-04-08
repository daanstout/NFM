using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace NFM_Core.Core;

public static class Time {
    public static TimeSpan DeltaTimeSpan { get; private set; }

    public static float DeltaTime { get; private set; }

    public static TimeSpan FixedDeltaTimeSpan { get; private set; }

    public static float FixedDeltaTime { get; set; }

    internal static void Update(GameTime delta) {
        DeltaTimeSpan = delta.ElapsedGameTime;
        DeltaTime = (float)delta.ElapsedGameTime.TotalSeconds;
    }

    internal static void FixedUpdate(TimeSpan delta) {
        FixedDeltaTimeSpan = delta;
        FixedDeltaTime = (float)delta.TotalSeconds;
    }
}
