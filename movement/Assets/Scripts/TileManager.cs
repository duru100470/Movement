using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonBehavior<TileManager>
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

            Debug.Log($"{tileHolder.Pos.X} {tileHolder.Pos.Y}");
        }
    }

    public List<TileHolder> GetTileHoldersDFS(Coordinate startPos)
    {
        List<TileHolder> tileHolderList = new List<TileHolder>();

        Debug.Log($"{startPos.X} {startPos.Y}");
        GetTileHoldersDFSRecursion(startPos, ref tileHolderList);

        return tileHolderList;
    }

    private void GetTileHoldersDFSRecursion(Coordinate pos, ref List<TileHolder> tileList)
    {
        tileList.Add(tileHolderDict[pos]);

        foreach (var kv in tileHolderDict)
        {
            if (Coordinate.Distance(kv.Key, pos) == 1)
            {
                if (!tileList.Contains(kv.Value))
                {
                    GetTileHoldersDFSRecursion(kv.Key, ref tileList);
                }
            }
        }
    }

    private void DestoryTile(Coordinate pos)
    {
        var target = tileHolderDict[pos];
        var ground = target.GetComponentInParent<Ground>();
        ground.RemoveTileHolder(target);

        Destroy(target);
    }

    public void RefreshDict(Dictionary<Coordinate, TileHolder> newDict)
    {
        foreach (var kv in newDict)
        {
            if (kv.Value == null)
                tileHolderDict.Remove(kv.Key);
            else
                tileHolderDict[kv.Key] = kv.Value;
        }
    }
}
