using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonBehavior<TileManager>
{
    private Dictionary<Coordinate, TileHolder> tileHolderDict;
    public Dictionary<Coordinate, TileHolder> TileHolderDict => tileHolderDict;
    private Dictionary<Entity, Coordinate> entityDict;
    private Dictionary<Entity, Coordinate> EntityDict => entityDict;
    public bool IsFirstLoading { get; set; } = true;

    private void Start()
    {
        tileHolderDict = new Dictionary<Coordinate, TileHolder>();
        entityDict = new Dictionary<Entity, Coordinate>();

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
            
            var entityObjs = GameObject.FindGameObjectsWithTag("Entity");

            foreach (var obj in entityObjs)
            {
                var entity = obj.GetComponent<Entity>();
                entityDict[entity] = entity.Pos;
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
            if (Coordinate.Distance(kv.Key, pos) == 1 && kv.Value.GetComponentInParent<Ground>().IsMergeable)
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
        TileHolder target;
        if (!tileHolderDict.TryGetValue(pos, out target))
        {
            var ground = target.GetComponentInParent<Ground>();
            ground.RemoveTileHolder(target);

            Destroy(target);
        }
    }

    public void DestroyEntity(Entity entity)
    {
        var ground = entity.GetComponentInParent<Ground>();
        ground.RemoveEntity(entity);
        entityDict.Remove(entity);
        Destroy(entity.gameObject);
    }

    public void RefreshTileHolderDict(Dictionary<Coordinate, TileHolder> newDict)
    {
        foreach (var kv in newDict)
        {
            if (kv.Value == null)
                tileHolderDict.Remove(kv.Key);
            else
                tileHolderDict[kv.Key] = kv.Value;
        }
    }

    public void RefreshEntityDict()
    {
        List<KeyValuePair<Coordinate, Entity>> buffer = new List<KeyValuePair<Coordinate, Entity>>();

        foreach (var kv in entityDict)
        {
            buffer.Add(new KeyValuePair<Coordinate, Entity>(kv.Value, kv.Key));
        }

        entityDict.Clear();

        foreach (var kv in buffer)
        {
            entityDict[kv.Value] = kv.Key;
        }
    }
}
