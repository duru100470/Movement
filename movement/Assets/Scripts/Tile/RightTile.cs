using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTile : Tile
{
    private Coordinate direction = new(1, 0);
    public override void RunCommand(Ground ground)
    {
        ground.MoveEntity(direction);
        ground.MoveTileHolder(direction);
        
    }
}
