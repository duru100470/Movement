using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    private int currentWindow;

    public List<GameObject> stagePanels = new List<GameObject> ();
    public int progress;
    private void Awake()
    {
        // 스테이지 씬에서 스테이지를 클리어할 때 progress를 증가시킨다. (버그 방지 위해 증가보다 설정이 더 좋을 듯)
        progress = PlayerPrefs.GetInt("Progress", 0);
        Debug.Log($"Progress: {progress}");
        //이제 레벨 선택 씬에서는 프로그래스에 따라 Enable or disable이 정해진다.
    }

    private void Start()
    {
        currentWindow = 0;
        ShowStagePanel(currentWindow);
    }

    public void ShowLeftSide() {
        if (currentWindow == 0) return;
        else 
        { 
            ShowStagePanel(currentWindow - 1);
            currentWindow--;
        }
    }

    public void ShowRightSide() {
        if (currentWindow == 4) return;
        else
        {
            ShowStagePanel(currentWindow + 1);
            currentWindow++;
        }
    }

    private void ShowStagePanel(int stage) {
        for (int i = 0; i < stagePanels.Count; i++) {
            if (i == stage) stagePanels[i].SetActive(true);
            else stagePanels[i].SetActive(false);
        }
    }

}
