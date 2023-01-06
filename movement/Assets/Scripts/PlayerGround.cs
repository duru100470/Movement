using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGround : Ground
{
    private List<Action<Ground>> commandList;

    public void RunScript()
    {
        StartCoroutine(RunScriptRoutine());
    }

    public void StopScript()
    {
        StopCoroutine(RunScriptRoutine());
    }

    public IEnumerator RunScriptRoutine()
    {
        while(true)
        {
            for(int i = 0; i < commandList.Count; i++)
            {
                commandList[i](this);
                // 커맨드 실행 중 병합이 일어나면 Script를 새로 생성해야함

                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void GenerateScript()
    {
        // tileHolderList에서 commandList를 생성
    }
}
