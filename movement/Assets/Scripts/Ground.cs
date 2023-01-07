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
    public List<Entity> EntityList => entityList;
    private List<Action<Ground>> commandList;
    private int index = 0;
    public bool IsDestroyed { get; set; } = false;

    private List<TileHolder> arrangedTileHolderList;
    private List<Entity> arrangedPowerList;
    private Queue<Coordinate> mineAndLaserPosition;

    [SerializeField]
    private bool hasPower;

    private void Awake()
    {
        commandList = new List<Action<Ground>>();
        commandTileHolderList = new List<TileHolder>();
        tileHolderList = GetComponentsInChildren<TileHolder>().ToList();
        entityList = GetComponentsInChildren<Entity>().ToList();
        mineAndLaserPosition = new Queue<Coordinate>();
    }

    public IEnumerator RunScriptRoutine()
    {
        while (true)
        {
            while (index < commandList.Count)
            {
                Debug.Log(index);
                commandList[index](this);
                index++;

                yield return null;
            }
            index = 0;
        }
    }

    public void GenerateScript()
    {
        TileHolder curCmdTileHolder = null;

        if (commandTileHolderList.Count > 0)
            curCmdTileHolder = commandTileHolderList[index % commandTileHolderList.Count];

        commandList.Clear();
        commandTileHolderList.Clear();
        mineAndLaserPosition.Clear();

        // tileHolderList에서 commandList를 생성
        arrangedTileHolderList = tileHolderList.OrderByDescending(x => x.Pos.Y).ThenBy(x => x.Pos.X).ToList();

        foreach (TileHolder tileholder in arrangedTileHolderList)
        {
            if (tileholder.CurTile != null && tileholder.CurTile.TileType == TILE_TYPE.COMMAND)
            {
                commandTileHolderList.Add(tileholder);
                commandList.Add(tileholder.CurTile.RunCommand);
            }
            else if (tileholder.CurTile != null && tileholder.CurTile.TileType == TILE_TYPE.COMMAND) {
                commandTileHolderList.Add(tileholder);
                commandList.Add(tileholder.CurTile.RunCommand);
                mineAndLaserPosition.Enqueue(tileholder.Pos);
            }
        }

        if (curCmdTileHolder != null)
            index = commandTileHolderList.IndexOf(curCmdTileHolder);
    }

    public int GetPriority()
    {
        int priority;
        if (!hasPower) priority = -100;
        else {
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
            foreach (var item in entityList) {
                if (item.EntityType == ENTITY_TYPE.POWER) {
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
        if (IsDestroyed) return;
        var result = TileManager.Inst.GetTileHoldersDFS(tileHolderList[0].Pos);

        if (result.Count <= tileHolderList.Count) return;

        foreach (var tileHolder in result)
        {
            var ground = tileHolder.GetComponentInParent<Ground>();
            if (this.gameObject != ground.gameObject)
                ground.IsDestroyed = true;

            tileHolder.gameObject.transform.SetParent(this.gameObject.transform);
        }

        tileHolderList = result;
        hasPower = true;
        GenerateScript();
    }

    // 이 Ground와 Steel Ground간의 충돌체크
    private bool CheckCollision(Coordinate pos)
    {
        return false;
    }

    public void RemoveTileHolder(TileHolder tileHolder) => tileHolderList.Remove(tileHolder);

    public void OperateLaser(int direction) {
        Coordinate laserPos = mineAndLaserPosition.Dequeue();

        // Laser 작동 코드
    }

    public void OperateMine() {
        Coordinate minePos = mineAndLaserPosition.Dequeue();

        // 지뢰 작동 코드. 
    }
}
