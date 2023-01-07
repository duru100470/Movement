using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonBehavior<TileManager>
{
    private Dictionary<Coordinate, TileHolder> tileHolderDict;
    public Dictionary<Coordinate, TileHolder> TileHolderDict => tileHolderDict;
    private Dictionary<Coordinate, Entity> entityDict;
    private Dictionary<Coordinate, Entity> EntityDict => entityDict;
    public bool IsFirstLoading { get; set; } = true;

    private void Start()
    {
        tileHolderDict = new Dictionary<Coordinate, TileHolder>();
        entityDict = new Dictionary<Coordinate, Entity>();

        // For Debug
        LoadCurrnetMapInfo();
    }

    public void SaveCurrentMapInfo()
    {
    }

    public void LoadCurrnetMapInfo()
    {
        tileHolderDict.Clear();
        entityDict.Clear();

        if (IsFirstLoading)
        {
            var objs = GameObject.FindGameObjectsWithTag("TileHolder");

            foreach (var obj in objs)
            {
                var tileHolder = obj.GetComponent<TileHolder>();
                tileHolderDict[tileHolder.Pos] = tileHolder;
            }
        }
        else
        {
            
        }
    }

    public List<TileHolder> GetTileHoldersDFS(Coordinate startPos)
    {
        List<TileHolder> tileHolderList = new List<TileHolder>();

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

    public void DestroyTile(Coordinate pos)
    {
        var target = tileHolderDict[pos];
        if (target != null)
        {
            var ground = target.GetComponentInParent<Ground>();
            ground.RemoveTileHolder(target);

            Destroy(target);
        }
    }

    public void DestroyEntity(Entity entity)
    {
        Coordinate pos = entity.Pos;
        var target = tileHolderDict[pos];
        if (target == null)
        {
            var ground = target.GetComponentInParent<Ground>();
            ground.RemoveEntity(entity);
            Destroy(entity);
        }
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SaveCurrentMapInfo();
        }
    }
}
