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
            groundRoutineList.ForEach(kv => { kv.Key.MergeGround(); });
            RefreshList();
            yield return new WaitForSeconds(1f); // 기획대로라면 슬라이더(or 토글버튼)로 턴 실행 시간을 바꿔야 하기 때문에 이 부분 수정 필요
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
            if (kv.Key.IsDestroyed)
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

        foreach (var obj in objList)
        {
            var ground = obj.GetComponent<Ground>();
            ground.GenerateScript();
            groundRoutineList.Add(new KeyValuePair<Ground, IEnumerator>(ground, ground.RunScriptRoutine()));
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
