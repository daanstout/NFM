using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFM_Core.Physics.Impulse;

public class Collider {
    public RigidBody RigidBody { get; internal set; }

    private protected Collider() {

    }
}
