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

    private List<Coordinate> destroyPositionList;
    public List<Coordinate> DestroyPositionList => destroyPositionList;
    public List<Entity> EntityList => entityList;
    private List<Action<Ground>> commandList;
    private int index = 0;
    public bool IsDestroyed { get; set; } = false;
    [SerializeField]
    private bool isMergeable;
    public bool IsMergeable => isMergeable;

    private List<TileHolder> arrangedTileHolderList;

    [SerializeField]
    private bool hasPower;

    private void Awake()
    {
        commandList = new List<Action<Ground>>();
        commandTileHolderList = new List<TileHolder>();
        tileHolderList = GetComponentsInChildren<TileHolder>().ToList();
        entityList = GetComponentsInChildren<Entity>().ToList();
<<<<<<< Updated upstream
=======
        mineAndLaserPosition = new Queue<Coordinate>();
        destroyPositionList = new List<Coordinate>();
>>>>>>> Stashed changes
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
        if (IsDestroyed || !IsMergeable) return;
        var result = TileManager.Inst.GetTileHoldersDFS(tileHolderList[0].Pos);
        List<TileHolder> tileHolderListBuffer = new List<TileHolder>();

        if (result.Count <= tileHolderList.Count) return;

        foreach (var tileHolder in result)
        {
            var ground = tileHolder.GetComponentInParent<Ground>();
            
            if (!ground.IsMergeable) continue;

            if (this.gameObject != ground.gameObject)
                ground.IsDestroyed = true;

            tileHolder.gameObject.transform.SetParent(this.gameObject.transform);
            tileHolderListBuffer.Add(tileHolder);
        }

        tileHolderList = tileHolderListBuffer;
        hasPower = true;
        GenerateScript();
    }

    // 이 Ground와 다른 Ground간의 충돌체크
    private bool CheckCollision(Coordinate pos)
    {
<<<<<<< Updated upstream
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
=======
        return false;
    }

    public void RemoveTileHolder(TileHolder tileHolder) => tileHolderList.Remove(tileHolder);
    public void RemoveEntity(Entity entity) => entityList.Remove(entity);

    public void OperateLaser(Coordinate direction) {
        Coordinate laserPos = mineAndLaserPosition.Dequeue();

        // Laser 작동 코드
        Coordinate newPos = laserPos + direction;
        for(int i = 0; i < 20; i++)
        {
            destroyPositionList.Add(newPos);
            newPos += direction;
        }
    }

    public void OperateMine() {
        Coordinate minePos = mineAndLaserPosition.Dequeue();

        destroyPositionList.Add(minePos);
        destroyPositionList.Add(minePos + new Coordinate(1, 0));
        destroyPositionList.Add(minePos + new Coordinate(0, 1));
        destroyPositionList.Add(minePos + new Coordinate(-1, 0));
        destroyPositionList.Add(minePos + new Coordinate(0, -1));

    }

    public void DestroyTileHolders()
    {
        foreach(var pos in destroyPositionList)
        {
            TileManager.Inst.DestroyTile(pos);
        }
    }

    public void CheckEntities()
    {
        foreach(var entity in entityList)
        {
            TileManager.Inst.DestroyEntity(entity);
        }
    }
>>>>>>> Stashed changes
}
