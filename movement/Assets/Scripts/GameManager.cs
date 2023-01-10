using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehavior<GameManager>
{
    public bool playing;
    public float gameSpeed = 2f;
    public TurnManager turnManager { get; set; }
    public UIManager uiManager { get; set; }
    public PlayerDataManager playerDataManager { get; set; }

    public GameObject bg;

    public GameObject exp_prefab;
    public GameObject laser_prefab;

    public void ClearStage()
    {
        Debug.Log("STAGE CLEAR");
        playing = false;
        playerDataManager.SetProgress();
        uiManager.ShowClearPanel();
    }

    public void Fail()
    {
        Debug.Log("FAIL");
        bg.GetComponent<SpriteRenderer>().color = Color.red;
        playing = false;
        uiManager.ShowFailPanel();
    }
    private void Start()
    {
        playing = false;
        playerDataManager = FindObjectOfType<PlayerDataManager>().GetComponent<PlayerDataManager>();
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        turnManager = FindObjectOfType<TurnManager>().GetComponent<TurnManager>();

        exp_prefab = Resources.Load("Explosion") as GameObject;
        laser_prefab = Resources.Load("Laser") as GameObject;
    }
}
