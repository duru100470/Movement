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

    [SerializeField]
    private bool hasPower;

    private void Awake()
    {
        commandList = new List<Action<Ground>>();
        commandTileHolderList = new List<TileHolder>();
        tileHolderList = GetComponentsInChildren<TileHolder>().ToList();
        entityList = GetComponentsInChildren<Entity>().ToList();
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

        // tileHolderList에서 commandList를 생성
        arrangedTileHolderList = tileHolderList.OrderByDescending(x => x.Pos.Y).ThenBy(x => x.Pos.X).ToList();

        foreach (TileHolder tileholder in arrangedTileHolderList)
        {
            if (tileholder.CurTile != null && tileholder.CurTile.TileType == TILE_TYPE.COMMAND)
            {
                commandTileHolderList.Add(tileholder);
                commandList.Add(tileholder.CurTile.RunCommand);
            }
        }

        if (curCmdTileHolder != null)
            index = commandTileHolderList.IndexOf(curCmdTileHolder);
    }

    public int GetPriority()
    {
        return 0;
    }

    public bool CheckHasPowerSource()
    {
        // return entityList.Count != 0;
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
        // Entity가 구현되면 사용 가능

        /*if (CheckCollision(pos)) return;

        foreach (var entity in EntityList)
        {
            entity.Pos += pos;
            entity.transform.position = Coordinate.CoordinatetoWorldPoint(tileHolder.Pos);
        }*/
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
}
