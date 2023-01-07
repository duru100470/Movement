using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDownTile : Tile
{
    public override void RunCommand(Ground ground)
    {
        ground.OperateLaser(new Coordinate(0, -1));
    }
}
