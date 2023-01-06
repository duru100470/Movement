using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpTile : Tile
{
    private Coordinate direction = new(0, 1);
    public override void RunCommand(Ground ground) {
        ground.MoveTileHolder(direction);
        ground.MoveEntity(direction);
    }
}
