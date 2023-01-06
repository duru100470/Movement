using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<Ground> groundList;
    private List<IEnumerator> routineList;

    private void Awake()
    {
        groundList = new List<Ground>();
    }

    public void Start()
    {
        RefreshList();
        StartCoroutine(TurnRoutine());
    }
    
    public void Stop()
    {
        StopCoroutine(TurnRoutine());
    }

    private IEnumerator TurnRoutine()
    {
        while(true)
        {
            DoCurrentTurn();
            groundList.ForEach(g => {g.MergeGround();});
            RefreshList();
            yield return new WaitForSeconds(1f);
        }
    }

    private void DoCurrentTurn()
    {
        routineList.ForEach(i => {i.MoveNext();});
    }

    private void RefreshList()
    {
        // refresh ground list and generate routine list by using ground list
    }
}
