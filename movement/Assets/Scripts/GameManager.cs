using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehavior<GameManager>
{
    public float gameSpeed = 0.25f;
    public TurnManager turnManager { get; set; }
    public UIManager uiManager { get; set; }
    public GameObject bg;

    public PlayerDataManager playerDataManager { get; set; }
    public void ClearStage()
    {
        Debug.Log("STAGE CLEAR");
        turnManager.StopRoutine();
        playerDataManager.SetProgress();
        uiManager.ShowClearPanel();
    }

    private void Start()
    {
        playerDataManager = FindObjectOfType<PlayerDataManager>().GetComponent<PlayerDataManager>();
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        turnManager = FindObjectOfType<TurnManager>().GetComponent<TurnManager>();
    }

    public void Fail()
    {
        Debug.Log("FAIL");
        bg.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void Awake() {
        Debug.Log(Coordinate.Distance(new Coordinate(0, 0), new Coordinate(1, 1)));
    }
}
