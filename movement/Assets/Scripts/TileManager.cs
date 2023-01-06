using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Dictionary<Coordinate, TileHolder> tileHolderDict;
    public Dictionary<Coordinate, TileHolder> TileHolderDict => tileHolderDict;

    private void Start()
    {
        tileHolderDict = new Dictionary<Coordinate, TileHolder>();

        var objs = GameObject.FindGameObjectsWithTag("TileHolder");

        foreach(var obj in objs)
        {
            var tileHolder = obj.GetComponent<TileHolder>();
            tileHolderDict[tileHolder.Pos] = tileHolder;
        }
    }

    private void DestoryTile(Coordinate pos)
    {
        var target = tileHolderDict[pos];
        var ground = target.GetComponentInParent<Ground>();
        ground.RemoveTileHolder(target);

        Destroy(target);
    }
}
