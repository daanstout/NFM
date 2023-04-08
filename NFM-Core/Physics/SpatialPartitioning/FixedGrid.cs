using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using NFM_Core.Physics.Verlet;

namespace NFM_Core.Physics.SpatialPartitioning;

public class FixedGrid : ISpatialPartitioner {
    private readonly int cellWidth;
    private readonly int cellHeight;

    private readonly int width;
    private readonly int height;

    private readonly Vector2 offset;

    private readonly List<PhysicsBody>[,] cells;

    public FixedGrid(int cellWidth, int cellHeight, int width, int height, Vector2 offset) {
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
        this.width = width;
        this.height = height;
        this.offset = offset;

        cells = new List<PhysicsBody>[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                cells[x, y] = new List<PhysicsBody>();
            }
        }
    }

    public IEnumerable<PhysicsBody> GetBodiesNearBody(PhysicsBody body) {
        int startX = body.CellX > 0 ? body.CellX - 1 : body.CellX;
        int startY = body.CellY > 0 ? body.CellY - 1 : body.CellY;

        int endX = body.CellX < width - 1 ? body.CellX + 1 : body.CellX;
        int endY = body.CellY < height - 1 ? body.CellY + 1 : body.CellY;

        for (int x = startX; x < endX; x++) {
            for (int y = startY; y < endY; y++) {
                for (int i = 0; i < cells[x, y].Count; i++) {
                    if (cells[x, y][i] != body)
                        yield return cells[x, y][i];
                }
            }
        }
    }

    public void UpdateBodyCell(PhysicsBody body) {
        RemoveBody(body);
        AddBody(body);
    }

    public void AddBody(PhysicsBody body) {
        int cellX = (int)(body.Transform.Position.X - offset.X);
        int cellY = (int)(body.Transform.Position.Y - offset.Y);

        cellX /= cellWidth;
        cellY /= cellHeight;

        cells[cellX, cellY].Add(body);
        body.CellX = cellX;
        body.CellY = cellY;
    }

    public void RemoveBody(PhysicsBody body) {
        cells[body.CellX, body.CellY].Remove(body);
    }
}
