using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : Tile
{
    public override void RunCommand(Ground ground, Coordinate pos)
    {
        for(int i=0;i< ground.EntityList.Count; i++)
        {
            if(ground.EntityList[i].EntityType == ENTITY_TYPE.POWER)
            {
                Debug.Log("Clear!!!");
            }
        }
    }
}
