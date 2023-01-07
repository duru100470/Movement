using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTile : Tile
{
    public override void RunCommand(Ground ground)
    {
        GameManager.Inst.Fail();
    }
}
