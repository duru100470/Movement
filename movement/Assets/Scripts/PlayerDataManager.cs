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
        // �������� ������ ���������� Ŭ������ �� progress�� ���� �����Ѵ�
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
    }
}
