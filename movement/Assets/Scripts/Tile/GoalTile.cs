using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : Tile
{
    public override void RunCommand(Ground ground, Coordinate pos)
    {
        ground.CheckStageClear();
    }
}
