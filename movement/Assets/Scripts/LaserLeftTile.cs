using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLeftTile : Tile
{
    public Coordinate direction = new(-1, 0);
    public override void RunCommand(Ground ground)
    {
        ground.OperateLaser(direction);
    }
}
