using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTile : Tile
{
    public override void RunCommand(Ground ground)
    {
<<<<<<< Updated upstream:movement/Assets/Scripts/Tile/LaserTile.cs
=======
        ground.OperateLaser(new Coordinate(0, 1));
>>>>>>> Stashed changes:movement/Assets/Scripts/Tile/LaserUpTile.cs
    }
}
