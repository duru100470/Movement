using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public PlayerDataManager pdm;

    [SerializeField]
    private int level;

    private void Start()
    {
        if (pdm.progress < level) gameObject.GetComponent<Button>().interactable = false;
    }
}
