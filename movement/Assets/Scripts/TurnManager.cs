using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private List<KeyValuePair<Ground, IEnumerator>> groundRoutineList;

    private void Start()
    {
        GetList();
    }

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
        while(true)
        {
            DoCurrentTurn();
            groundRoutineList.ForEach(g => {g.Key.MergeGround();});
            RefreshList();
            yield return new WaitForSeconds(1f);
        }
    }

    private void DoCurrentTurn()
    {
        groundRoutineList.ForEach(kv => {
            if(kv.Key.CheckHasPowerSource())
            {
                kv.Value.MoveNext();
            }
        });
    }

    private void RefreshList()
    {
        List<KeyValuePair<Ground, IEnumerator>> groundRoutineListBuffer 
            = new List<KeyValuePair<Ground, IEnumerator>>();
        
        // refresh ground list and generate routine list by using ground list
        foreach(var kv in groundRoutineList)
        {
            if(kv.Key == null)
            {
                groundRoutineList.Add(kv);
            }
        }

        foreach(var kv in groundRoutineListBuffer)
        {
            groundRoutineList.Remove(kv);
        }
    }

    private void GetList()
    {
        groundRoutineList = new List<KeyValuePair<Ground, IEnumerator>>();

        var objList = GameObject.FindGameObjectsWithTag("Ground");

        foreach(var obj in objList)
        {
            var ground = obj.GetComponent<Ground>();
            groundRoutineList.Add(new KeyValuePair<Ground, IEnumerator>(ground, ground.RunScriptRoutine()));
        }

        groundRoutineList.OrderBy(x => x.Key.GetPriority());
    }
}
