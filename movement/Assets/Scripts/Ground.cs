using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private List<TileHolder> tileHolderList;
    public List<TileHolder> TileHolderList => tileHolderList;
    private List<TileHolder> commandTileHolderList;
    private List<Entity> entityList;

    private List<TileHolder> destroyTileHolderList;
    public List<TileHolder> DestroyTileHolderList => destroyTileHolderList;
    public List<Entity> EntityList => entityList;
    private List<Action<Ground, Coordinate>> commandList;
    private int index = 0;
    public bool IsDestroyed { get; set; } = false;
    [SerializeField]
    private bool isMergeable;
    public bool IsMergeable => isMergeable;
    [SerializeField]
    private bool hasPower;

    private List<TileHolder> arrangedTileHolderList;
    private List<Entity> arrangedPowerList;

    private void Awake()
    {
        commandList = new List<Action<Ground, Coordinate>>();
        commandTileHolderList = new List<TileHolder>();
        tileHolderList = GetComponentsInChildren<TileHolder>().ToList();
        entityList = GetComponentsInChildren<Entity>().ToList();
        destroyTileHolderList = new List<TileHolder>();

    }

    public IEnumerator RunScriptRoutine()
    {
        while (true)
        {
            while (index < commandList.Count)
            {

                commandList[index](this, commandTileHolderList[index].Pos);
                index++;

                yield return null;
            }
            index = 0;
        }
    }

    public void GenerateScript()
    {
        List<TileHolder> lastCmdTileHolderList = commandTileHolderList.ToList();
        if (lastCmdTileHolderList.Count > 0)
            index = index % lastCmdTileHolderList.Count;

        commandList.Clear();
        commandTileHolderList.Clear();

        // tileHolderList에서 commandList를 생성
        arrangedTileHolderList = tileHolderList.OrderByDescending(x => x.Pos.Y).ThenBy(x => x.Pos.X).ToList();

        foreach (var tileholder in arrangedTileHolderList)
        {
            if (tileholder.CurTile != null && tileholder.CurTile.TileType == TILE_TYPE.COMMAND)
            {
                commandTileHolderList.Add(tileholder);
                commandList.Add(tileholder.CurTile.RunCommand);
            }
        }
        for(int i = index; i<lastCmdTileHolderList.Count; i++)
        {

            if (lastCmdTileHolderList[i] != null && commandTileHolderList.Contains(lastCmdTileHolderList[i]))
            {
                index = commandTileHolderList.IndexOf(lastCmdTileHolderList[i]);
                break;
            }
        }
        if (index >= lastCmdTileHolderList.Count) index = 0;
    }

    public int GetPriority()
    {
        int priority;
        if (!hasPower) priority = -100;
        else
        {
            arrangedPowerList = entityList.Where(entity => entity.EntityType == ENTITY_TYPE.POWER).
                                           OrderByDescending(entity => entity.Pos.Y).
                                           ThenBy(entity => entity.Pos.X).ToList();
            priority = (arrangedPowerList[0].Pos.Y * 100) - (arrangedPowerList[0].Pos.X);
        }
        return priority;
    }

    public bool CheckHasPowerSource()
    {
        if (entityList.Count == 0) hasPower = false;
        else
        {
            hasPower = entityList.Exists(entity => entity.EntityType == ENTITY_TYPE.POWER);
            foreach (var item in entityList)
            {
                if (item.EntityType == ENTITY_TYPE.POWER)
                {
                    Debug.Log($"There is a power at position {item.Pos.X}, {item.Pos.Y}");
                }
            }
        }
        return hasPower;
    }

    public virtual void MoveTileHolder(Coordinate pos)
    {
        if (CheckCollision(pos)) return;

        Dictionary<Coordinate, TileHolder> newTileHolderDict = new Dictionary<Coordinate, TileHolder>();

        foreach (var tileHolder in tileHolderList)
        {
            newTileHolderDict[tileHolder.Pos] = null;
        }

        foreach (var tileHolder in tileHolderList)
        {
            tileHolder.Pos += pos;
            if (tileHolder.CurTile != null && tileHolder.CurTile.TileType == TILE_TYPE.WALL)
            {
                foreach (var entity in entityList)
                {
                    if (entity.Pos == tileHolder.Pos)
                    {
                        entity.Pos += pos;
                        entity.transform.position = Coordinate.CoordinatetoWorldPoint(entity.Pos);
                    }
                }
            }
            tileHolder.transform.position = Coordinate.CoordinatetoWorldPoint(tileHolder.Pos);
            newTileHolderDict[tileHolder.Pos] = tileHolder;
        }

        TileManager.Inst.RefreshDict(newTileHolderDict);
    }

    public virtual void MoveEntity(Coordinate pos)
    {
        // 미완성
        Dictionary<Coordinate, Entity> newEntityDict = new Dictionary<Coordinate, Entity>();

        foreach (var entity in entityList)
        {
            newEntityDict[entity.Pos] = null;
        }

        foreach (var entity in entityList)
        {
            entity.Pos += pos;
            entity.transform.position = Coordinate.CoordinatetoWorldPoint(entity.Pos);
            newEntityDict[entity.Pos] = entity;
        }

        // TileManager처럼 Refresh를 구현하지 않아서 Dictionary가 더러움
    }

    public void MergeGround()
    {
        if (!IsMergeable) return;
        var endList = new List<TileHolder>();
        var groundList = new List<List<TileHolder>>();
        List<List<Entity>> entityListBuffer = new List<List<Entity>>();
        for (int i=0; i<tileHolderList.Count; i++)
        {
            if (tileHolderList[i].gameObject == null || endList.Contains(tileHolderList[i])) continue;
            var result = TileManager.Inst.GetTileHoldersDFS(tileHolderList[i].Pos);
            var entityResult = new List<Entity>();
            groundList.Add(new List<TileHolder>());
            foreach (var tileHolder in result)
            {
                endList.Add(tileHolder);
                var ground = tileHolder.GetComponentInParent<Ground>();

                if (!ground.IsMergeable)
                {
                    continue;
                }

                if(this != ground)
                {
                    ground.RemoveTileHolder(tileHolder);
                }

                foreach(var entity in ground.EntityList)
                {
                    if (entity.gameObject == null) continue;
                    if(entity.Pos == tileHolder.Pos)
                    {
                        entityResult.Add(entity);
                    }
                }

                groundList[i].Add(tileHolder);
            }
            entityListBuffer.Add(entityResult);
        }

        tileHolderList = new List<TileHolder>();
        entityList = new List<Entity>();
        int myIndex = 0;
        foreach(var list in groundList)
        {
            if (list.Contains(commandTileHolderList[index%commandTileHolderList.Count]))
            {
                myIndex = groundList.IndexOf(list);
            }
        }
        for(int i = 0; i < groundList.Count; i++)
        {
            Ground ground;
            if (i == myIndex)
            {
                ground = this;
            }
            else
            {
                GameObject newGround = Instantiate(new GameObject("Ground"));
                newGround.tag = "Ground";
                newGround.AddComponent<Ground>();
                ground = newGround.GetComponent<Ground>();
            }
            foreach(var tileHolder in groundList[i])
            {
                tileHolder.gameObject.transform.SetParent(ground.gameObject.transform);
                ground.tileHolderList.Add(tileHolder);
                Debug.Log("tile: " + tileHolder.name);
            }
            foreach(var entity in entityListBuffer[i])
            {
                entity.gameObject.transform.SetParent(ground.gameObject.transform);
                ground.entityList.Add(entity);
                Debug.Log("entity: " + entity.name);
            }
        }

        hasPower = true;
        GenerateScript();
    }

    // 이 Ground와 다른 Ground간의 충돌체크
    private bool CheckCollision(Coordinate pos)
    {
        bool ret = false;

        foreach (var tileHolder in tileHolderList)
        {
            var newPos = tileHolder.Pos + pos;
            TileHolder tmp;
            if (!TileManager.Inst.TileHolderDict.TryGetValue(newPos, out tmp)) continue;
            Debug.Log($"{newPos.X} {newPos.Y}");
            Debug.Log(tmp);
            if (tmp.GetComponentInParent<Ground>().gameObject != this.gameObject)
            {
                ret = true;
                break;
            }
        }

        return ret;
    }

    public void RemoveTileHolder(TileHolder tileHolder) => tileHolderList.Remove(tileHolder);

    public void RemoveEntity(TileHolder tileHolder) => tileHolderList.Remove(tileHolder);
    public void RemoveEntity(Entity entity) => entityList.Remove(entity);

    public void OperateLaser(Coordinate direction, Coordinate pos) {
        Coordinate laserPos = pos;

        // Laser 작동 코드
        Coordinate newPos = laserPos + direction;
        for(int i = 0; i < 20; i++)
        {
            destroyTileHolderList.Add(TileManager.Inst.FindTileHolder(newPos));
            newPos += direction;
        }
    }

    public void OperateMine(Coordinate pos) {
        Coordinate minePos = pos;

        destroyTileHolderList.Add(TileManager.Inst.FindTileHolder(pos));
        destroyTileHolderList.Add(TileManager.Inst.FindTileHolder(pos + new Coordinate(1, 0)));
        destroyTileHolderList.Add(TileManager.Inst.FindTileHolder(pos + new Coordinate(-1, 0)));
        destroyTileHolderList.Add(TileManager.Inst.FindTileHolder(pos + new Coordinate(0, 1)));
        destroyTileHolderList.Add(TileManager.Inst.FindTileHolder(pos + new Coordinate(0, -1)));
    }

    public void DestroyTileHolders()
    {
        List<TileHolder> newList = new List<TileHolder>();
        foreach (var tileHolder in destroyTileHolderList)
        {
            if (TileManager.Inst.DestroyTile(tileHolder))
            {
                newList.Add(tileHolder);
            }
        }
        foreach (var tileHolder in newList)
        {
            RemoveTileHolder(tileHolder);
        }
    }

    public void CheckEntities()
    {
        List<Entity> newList = new List<Entity>();
        for(int i=0;i<entityList.Count ;i++)
        {
            if (TileManager.Inst.DestroyEntity(entityList[i]))
            {
                newList.Add(entityList[i]);
            }
        }
        foreach(var entity in newList)
        {
            RemoveEntity(entity);
        }

    }
}
