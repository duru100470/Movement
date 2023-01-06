using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUpTile : Tile
{
    public Coordinate direction = new(0, 1);
    public override void RunCommand(Ground ground)
    {
        ground.OperateLaser(direction);
    }
}
