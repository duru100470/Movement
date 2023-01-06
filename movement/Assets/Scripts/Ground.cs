using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    protected List<TileHolder> tileHolderList;
    public List<TileHolder> TileHolderList => tileHolderList;
    protected List<Entity> entityList;
    public List<Entity> EntityList => entityList;

    public virtual void Move(Coordinate pos)
    {
        if(CheckCollision(pos)) return;

        foreach(var tileHolder in tileHolderList)
        {
            tileHolder.Pos += pos;
            tileHolder.transform.position = Coordinate.CoordinatetoWorldPoint(tileHolder.Pos);
        }

        MergeGround();
    }

    // 이 Ground와 Steel Ground간의 충돌체크
    private bool CheckCollision(Coordinate pos)
    {
        return false;
    }

    protected virtual void MergeGround()
    {
        var newGround = CheckGround();

        // 다른 Ground의 TileHolder를 이 Ground로 병합
    }

    // 이 Ground와 다른 Ground간의 인접체크
    // 인접한 Ground가 있다면 그 Ground 리턴
    private Ground CheckGround()
    {
        return null;
    }
}
