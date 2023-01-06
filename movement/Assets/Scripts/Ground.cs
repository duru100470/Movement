using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ground : MonoBehaviour
{
    private List<TileHolder> tileHolderList;
    public List<TileHolder> TileHolderList => tileHolderList;
    private List<Entity> entityList;
    public List<Entity> EntityList => entityList;
    private List<Action<Ground>> commandList;
    private int index = 0;

    private List<TileHolder> arrangedTileHolderList;

    private void Awake()
    {
        commandList = new List<Action<Ground>>();
        tileHolderList = GetComponentsInChildren<TileHolder>().ToList();
        entityList = GetComponentsInChildren<Entity>().ToList();
    }

    public IEnumerator RunScriptRoutine()
    {
        while(true)
        {
            for(int i = index; i < commandList.Count; i++)
            {
                commandList[i](this);
                index++;
                // 커맨드 실행 중 병합이나 파괴가 일어나면 Script를 새로 갱신해야함

                yield return null;
            }
            index = 0;
        }
    }

    public void GenerateScript()
    {
        // tileHolderList에서 commandList를 생성
        arrangedTileHolderList = tileHolderList.OrderByDescending(x => x.Pos.Y).ThenBy(x => x.Pos.X).ToList();

        foreach (TileHolder tileholder in arrangedTileHolderList) {
            if (tileholder.CurTile != null) commandList.Add(tileholder.CurTile.RunCommand);
        }
    }

    public int GetPriority()
    {
        return 0;
    }

    public bool CheckHasPowerSource()
    {
        return entityList.Count != 0;
    }

    public virtual void MoveTileHolder(Coordinate pos)
    {
        if(CheckCollision(pos)) return;

        foreach(var tileHolder in tileHolderList)
        {
            tileHolder.Pos += pos;
            tileHolder.transform.position = Coordinate.CoordinatetoWorldPoint(tileHolder.Pos);
        }
    }

    public virtual void MoveEntity(Coordinate pos) {
        // Entity가 구현되면 사용 가능

        /*if (CheckCollision(pos)) return;

        foreach (var entity in EntityList)
        {
            entity.Pos += pos;
            entity.transform.position = Coordinate.CoordinatetoWorldPoint(tileHolder.Pos);
        }*/
    }

    // 이 Ground와 Steel Ground간의 충돌체크
    private bool CheckCollision(Coordinate pos)
    {
        return false;
    }

    public void MergeGround()
    {
        var newGrounds = CheckGround();

        if(newGrounds == null) return;

        // 다른 Ground의 TileHolder를 이 Ground로 병합
    }

    // 이 Ground와 다른 Ground간의 인접체크
    // 인접한 Ground가 있다면 그 Ground 리턴
    private List<Ground> CheckGround()
    {
        List<Ground> adjacentGrounds = new List<Ground>();

        for(int x = -1; x < 2; x++)
            for(int y = -1; y < 2; y++)
            {
                var tempTileHolder = TileManager.Inst.TileHolderDict[new Coordinate(x, y)];
                var tempGround = tempTileHolder.GetComponentInParent<Ground>();

                if(tempGround != this) adjacentGrounds.Add(tempGround);
            }

        if (adjacentGrounds.Count == 0) return null;
        else return adjacentGrounds;
    }

    public void RemoveTileHolder(TileHolder tileHolder) => tileHolderList.Remove(tileHolder);

    // Code for debug
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Move(new Coordinate(0, 1));
        }
    }

}
