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
    private List<Action<Ground, Coordinate>> commandList;
    private int index = 0;
    public bool IsDestroyed { get; set; } = false;
    [SerializeField]
    private bool isMergeable;
    public bool IsMergeable => isMergeable;
    [SerializeField]
    private bool hasPower;

    private List<Tile> highlightCancelationBuffer;
    private List<TileHolder> arrangedTileHolderList;
    private List<Entity> arrangedPowerList;
    private void Awake()
    {
        commandList = new List<Action<Ground, Coordinate>>();
        commandTileHolderList = new List<TileHolder>();
        tileHolderList = GetComponentsInChildren<TileHolder>().ToList();
        entityList = GetComponentsInChildren<Entity>().ToList();
        destroyPositionList = new List<Coordinate>();
        highlightCancelationBuffer = new List<Tile>();
    }

    public IEnumerator RunScriptRoutine()
    {
        while (true)
        {
            //if (commandList.Count == 0) yield break;
            if (commandList.Count == 0)
            {
                yield return new WaitUntil(() => commandList.Count > 0);
            }

            while (index < commandList.Count)
            {
                //if (commandList.Count == 0) yield break;

                Debug.Log(index);
                commandList[index](this, commandTileHolderList[index].Pos);
                commandTileHolderList[index].CurTile.IsRunning = true;

                yield return null;
                //if (commandList.Count == 0) yield break;
                if (commandList.Count == 0)
                {
                    yield return new WaitUntil(() => commandList.Count > 0);
                }
                commandTileHolderList[index].CurTile.IsRunning = false;

                for (int i = highlightCancelationBuffer.Count - 1; i >= 0; i--)
                {
                    highlightCancelationBuffer[i].IsRunning = false;
                    highlightCancelationBuffer.RemoveAt(i);
                }

                index++;
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

        foreach (var tileholder in arrangedTileHolderList)
        {
            if (tileholder.CurTile != null && (tileholder.CurTile.TileType == TILE_TYPE.COMMAND || tileholder.CurTile.TileType == TILE_TYPE.GOAL))
            {
                commandTileHolderList.Add(tileholder);
                commandList.Add(tileholder.CurTile.RunCommand);
            }
        }

        if (curCmdTileHolder != null)
            index = commandTileHolderList.IndexOf(curCmdTileHolder);

        if(commandTileHolderList.Count > 0 && index >= commandTileHolderList.Count)
        {
            index %= commandTileHolderList.Count;
            Debug.Log(commandTileHolderList[index] == null);
        }
    }


    public int GetPriority()
    {
        int priority;
        CheckHasPowerSource();

        hasPower = CheckHasPowerSource();
        if (!hasPower) priority = Int32.MinValue;
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
        }
        return hasPower;
    }

    public virtual void MoveTileHolder(Coordinate pos)
    {
        if (CheckCollision(pos)) return;

        SoundManager.Inst.PlayEffectSound(SOUND_NAME.CHECK_SOUND, 1f, 1f);
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

        TileManager.Inst.RefreshTileHolderDict(newTileHolderDict);
    }

    public virtual void MoveEntity(Coordinate pos)
    {
        if (CheckCollision(pos)) return;

        foreach (var entity in entityList)
        {
            entity.Pos += pos;
            entity.transform.position = Coordinate.CoordinatetoWorldPoint(entity.Pos);
        }

        TileManager.Inst.RefreshEntityDict();
    }

    /*public void MergeGround()
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
            {
                ground.IsDestroyed = true;
                foreach (var entity in ground.entityList)
                {
                    entity.gameObject.transform.SetParent(this.gameObject.transform);
                    entityList.Add(entity);
                }
                ground.entityList.Clear();

                // Merge시 하이라이팅 픽스
                if (tileHolder.CurTile != null && tileHolder.CurTile.IsRunning)
                    highlightCancelationBuffer.Add(tileHolder.CurTile);
            }
            tileHolder.gameObject.transform.SetParent(this.gameObject.transform);
            tileHolderListBuffer.Add(tileHolder);
        }

        tileHolderList = tileHolderListBuffer;
        hasPower = true;
        CheckStageClear();
        GenerateScript();
    } */

    public void MergeGround()
    {
        if (!IsMergeable) return;
        var endList = new List<TileHolder>();
        var groundList = new List<List<TileHolder>>();
        List<List<Entity>> entityListBuffer = new List<List<Entity>>();
        int newIndex = 0;
        for (int i = 0; i < tileHolderList.Count; i++)
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

                if (this != ground)
                {
                    ground.RemoveTileHolder(tileHolder);
                }

                foreach (var entity in ground.EntityList)
                {
                    if (entity.gameObject == null) continue;
                    if (entity.Pos == tileHolder.Pos)
                    {
                        entityResult.Add(entity);
                    }
                }
                groundList[newIndex].Add(tileHolder);
            }

            newIndex++;
            entityListBuffer.Add(entityResult);
        }

        tileHolderList = new List<TileHolder>();
        entityList = new List<Entity>();
        int myIndex = 0;

        foreach (var list in groundList)
        {
            if (commandTileHolderList.Count > 0 && list.Contains(commandTileHolderList[(index % commandTileHolderList.Count)]))
            {
                myIndex = groundList.IndexOf(list);
            }
        }
        for (int i = 0; i < groundList.Count; i++)
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
                ground.isMergeable = true;
            }
            foreach (var tileHolder in groundList[i])
            {
                tileHolder.gameObject.transform.SetParent(ground.gameObject.transform);
                ground.tileHolderList.Add(tileHolder);
                Debug.Log("tile: " + tileHolder.name);
            }
            foreach (var entity in entityListBuffer[i])
            {
                entity.gameObject.transform.SetParent(ground.gameObject.transform);
                ground.entityList.Add(entity);
                Debug.Log("entity: " + entity.name);
            }
        }


        hasPower = true;
        GenerateScript();
    }

    public void CheckStageClear()
    {
        Debug.Log(tileHolderList.Exists(x => x.CurTile != null && x.CurTile.TileType == TILE_TYPE.GOAL));
        Debug.Log(entityList.Exists(x => (x is Power) && (x as Power).IsPlayer));

        if (tileHolderList.Exists(x => x.CurTile != null && x.CurTile.TileType == TILE_TYPE.GOAL) &&
            entityList.Exists(x => (x is Power) && (x as Power).IsPlayer))
        {
            SoundManager.Inst.PauseAll();
            SoundManager.Inst.PlayEffectSound(SOUND_NAME.CLEAR_SOUND, 1f, 1f);
            GameManager.Inst.ClearStage();
        }
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
            if (tmp.GetComponentInParent<Ground>().gameObject != this.gameObject)
            {
                ret = true;
                break;
            }
        }

        return ret;
    }

    public void RemoveTileHolder(TileHolder tileHolder) => tileHolderList.Remove(tileHolder);

    /* public void OperateLaser(int direction)
    {
        Coordinate laserPos = mineAndLaserPosition.Dequeue();


    } */

    public void RemoveEntity(Entity entity) => entityList.Remove(entity);

    public void OperateLaser(Coordinate direction, Coordinate pos)
    {
        SoundManager.Inst.PlayEffectSound(SOUND_NAME.LASER_SOUND, 0.2f, 1f);
        Debug.Log($"Laser Operated at {pos.X},{pos.Y}");
        Coordinate laserPos = pos;
        // Laser 작동 코드
        Coordinate newPos = laserPos + direction;
        Coordinate summonPos = laserPos;
        for (int i = 0; i < 24; i++)
        {
            destroyPositionList.Add(newPos);
            newPos += direction;
            if(i == 11)
            {
                summonPos = newPos;
            }
        }
        TileManager.Inst.laser_shoot(summonPos, direction);
    }

    public void OperateMine(Coordinate pos)
    {
        SoundManager.Inst.PlayEffectSound(SOUND_NAME.BREAK_SOUND, 1f, 1f);
        Coordinate minePos = pos;

        destroyPositionList.Add(minePos);
        destroyPositionList.Add(minePos + new Coordinate(1, 0));
        destroyPositionList.Add(minePos + new Coordinate(0, 1));
        destroyPositionList.Add(minePos + new Coordinate(-1, 0));
        destroyPositionList.Add(minePos + new Coordinate(0, -1));

    }

    /* 이 함수의 원본
    public void DestroyTileHolders()
    {
        foreach (var pos in destroyPositionList)
        {
            TileManager.Inst.DestroyTile(pos);
        }
    } */

    public void DestroyTileHolders() {
        List<TileHolder> newList = new List<TileHolder>();
        Debug.Log("total: " + tileHolderList.Count);
        Debug.Log("To DESTROY: " + destroyPositionList.Count);
        foreach (var pos in destroyPositionList)
        {
            TileHolder tileHolder = TileManager.Inst.DestroyTile(pos);
            if (tileHolder != null)
            {
                newList.Add(tileHolder);
            }
        }
        Debug.Log("DESTROY: " + newList.Count);
        if (newList.Count > 0)
        {
            SoundManager.Inst.PlayEffectSound(SOUND_NAME.BREAK_SOUND, 1f, 1f);
        }
        foreach (var tileHolder in newList)
        {
            int newIndex = -1;
            if (commandTileHolderList.Contains(tileHolder))
            {
                newIndex = commandTileHolderList.IndexOf(tileHolder);
            }
            Debug.Log(tileHolder.gameObject.name);
            if (newIndex >= 0)
            {
                commandTileHolderList.Remove(tileHolder);

                Debug.Log("DESTROY!");
                commandList.Remove(commandList[newIndex]);
            }
            RemoveTileHolder(tileHolder);
            Debug.Log("DESTROY!");

            TileManager.Inst.mineExplode(tileHolder.transform.position);
            
            Destroy(tileHolder.gameObject);

            Debug.Log("DESTROY!");
        }
        destroyPositionList = new List<Coordinate>();
        GenerateScript();
    }

    public void CheckEntities()
    {
        var buffer = new List<Entity>();

        foreach (var entity in entityList)
        {
            if (!TileManager.Inst.TileHolderDict.ContainsKey(entity.Pos))
            {
                buffer.Add(entity);
            }
        }
        if(buffer.Count > 0)
        {
            SoundManager.Inst.PlayEffectSound(SOUND_NAME.FALLING_SOUND, 0.25f, 3f);
        }

        foreach (var entity in buffer)
        {
            if (TileManager.Inst.DestroyEntity(entity)) RemoveEntity(entity);
        }
    }

    
}
