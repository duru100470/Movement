using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehavior<GameManager>
{
    public float gameSpeed { get; set; } = 1f;
    public TurnManager turnManager { get; set; }
    public UIManager uiManager { get; set; }

    public void ClearStage()
    {
        Debug.Log("STAGE CLEAR");
    }
}
