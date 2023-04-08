using NFM_Core.Physics.Verlet;

namespace NFM_Core.Physics.SpatialPartitioning;
public interface ISpatialPartitioner {
    void AddBody(PhysicsBody body);
    IEnumerable<PhysicsBody> GetBodiesNearBody(PhysicsBody body);
    void RemoveBody(PhysicsBody body);
    void UpdateBodyCell(PhysicsBody body);
}