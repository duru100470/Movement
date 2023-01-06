using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRightTile : Tile
{
    public Coordinate direction = new(1, 0);
    public override void RunCommand(Ground ground)
    {
        ground.OperateLaser(direction);
    }
}
