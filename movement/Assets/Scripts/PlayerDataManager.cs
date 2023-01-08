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
        // 스테이지 씬에서 스테이지를 클리어할 때 progress를 새로 설정한다
        progress = PlayerPrefs.GetInt("Progress", 0);
        Debug.Log($"Progress: {progress}");
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

    public void ResetData() {
        PlayerPrefs.SetInt("Progress", 0);
        progress = 0;
    }

    public void SetProgress() {
        int curprogress = PlayerPrefs.GetInt("Progress");
        progress = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Progress", Mathf.Max(curprogress, progress));
    }
}
