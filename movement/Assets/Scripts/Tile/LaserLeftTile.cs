using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLeftTile : Tile
{
    public override void RunCommand(Ground ground)
    {
        ground.OperateLaser(new Coordinate(-1, 0));
    }
}
