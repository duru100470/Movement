using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public PlayerDataManager pdm;

    public int level;

    private void Start()
    {
        if (pdm.progress < level) gameObject.GetComponent<Button>().interactable = false;
    }

    public void SelectStageFromLevel() {
        SceneManager.LoadScene(level);
    }
}
