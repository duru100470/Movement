using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private List<TileHolder> tileHolderList;
    public List<TileHolder> TileHolderList => tileHolderList;
    private List<Entity> entityList;
    public List<Entity> EntityList => entityList;
    private List<Action<Ground>> commandList;
    private int index = 0;

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
    }

    public int GetPriority()
    {
        return 0;
    }

    public bool CheckHasPowerSource()
    {
        return entityList.Count != 0;
    }

    public virtual void Move(Coordinate pos)
    {
        if(CheckCollision(pos)) return;

        foreach(var tileHolder in tileHolderList)
        {
            tileHolder.Pos += pos;
            tileHolder.transform.position = Coordinate.CoordinatetoWorldPoint(tileHolder.Pos);
        }
    }

    // 이 Ground와 Steel Ground간의 충돌체크
    private bool CheckCollision(Coordinate pos)
    {
        return false;
    }

    public void MergeGround()
    {
        var newGround = CheckGround();

        if(newGround == null) return;

        // 다른 Ground의 TileHolder를 이 Ground로 병합
    }

    // 이 Ground와 다른 Ground간의 인접체크
    // 인접한 Ground가 있다면 그 Ground 리턴
    private Ground CheckGround()
    {
        return null;
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
