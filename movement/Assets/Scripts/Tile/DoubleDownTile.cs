using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDownTile : Tile
{
    private Coordinate direction = new(0, -1);
    public override void RunCommand(Ground ground, Coordinate pos)
    {
        ground.MoveTileHolder(direction);
    }
}
