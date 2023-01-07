using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleRightTile : Tile
{
    private Coordinate direction = new(1, 0);
    public override void RunCommand(Ground ground, Coordinate pos)
    {
        ground.MoveTileHolder(direction);
        ground.MoveEntity(direction);
    }
}
