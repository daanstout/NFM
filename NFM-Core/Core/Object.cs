using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFM_Core.Core;

public abstract class Object : IEquatable<Object?> {
    internal Guid ID { get; }

    protected Object() {
        ID = Guid.NewGuid();
    }

    public override bool Equals(object? obj) => Equals(obj as Object);
    public bool Equals(Object? other) => other is not null && ID.Equals(other.ID);
    public override int GetHashCode() => HashCode.Combine(ID);

    public static bool operator ==(Object? left, Object? right) => EqualityComparer<Object>.Default.Equals(left, right);
    public static bool operator !=(Object? left, Object? right) => !(left == right);
}
