using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private List<KeyValuePair<Ground, IEnumerator>> groundRoutineList;

    public void StartRoutine()
    {
        GetList();
        StartCoroutine(TurnRoutine());
    }

    public void StopRoutine()
    {
        StopCoroutine(TurnRoutine());
    }

    private IEnumerator TurnRoutine()
    {
        while (true)
        {
            DoCurrentTurn();
            groundRoutineList.ForEach(kv => { kv.Key.DestroyTileHolders(); });
            groundRoutineList.ForEach(kv => { kv.Key.CheckEntities(); });
            groundRoutineList.ForEach(kv => { kv.Key.MergeGround(); });
            RefreshList();
            yield return new WaitForSeconds(GameManager.Inst.gameSpeed);
        }
    }

    private void DoCurrentTurn()
    {
        groundRoutineList.ForEach(kv =>
        {
            if (kv.Key.CheckHasPowerSource())
            {
                kv.Value.MoveNext();
            }
        });
    }

    private void RefreshList()
    {
        List<KeyValuePair<Ground, IEnumerator>> groundListBuffer
            = new List<KeyValuePair<Ground, IEnumerator>>();

        // refresh ground list and generate routine list by using ground list
        foreach (var kv in groundRoutineList)
        {

            if (kv.Key.transform.childCount == 0)
            {
                groundListBuffer.Add(kv);
            }
        }

        for (int i = groundListBuffer.Count - 1; i >= 0; i--)
        {
            groundRoutineList.Remove(groundListBuffer[i]);
            Destroy(groundListBuffer[i].Key.gameObject);
        }
    }

    private void GetList()
    {
        groundRoutineList = new List<KeyValuePair<Ground, IEnumerator>>();

        var objList = GameObject.FindGameObjectsWithTag("Ground");

        var destroyList = new List<GameObject>();

        foreach (var obj in objList)
        {
            var ground = obj.GetComponent<Ground>();
            ground.GenerateScript();
            groundRoutineList.Add(new KeyValuePair<Ground, IEnumerator>(ground, ground.RunScriptRoutine()));
        }
        
        foreach (var obj in destroyList)
        {
            Destroy(obj);
        }

        groundRoutineList.OrderByDescending(x => x.Key.GetPriority());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRoutine();
        }
    }
}
