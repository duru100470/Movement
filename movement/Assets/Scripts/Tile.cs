using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField]
    private TILE_TYPE tileType;
    public TILE_TYPE TileType => tileType;
    private bool isRunning;
    public bool IsRunning {
        get
        {
            return isRunning;
        }
        set
        {
            GetComponent<SpriteRenderer>().color = 
                value ? new Color(1f, 1f, 1f, 1f) : new Color(.6f, .6f, .6f, 1f);
            isRunning = value;
        } 
    }
    public abstract void RunCommand(Ground ground);

    private void Awake()
    {
        IsRunning = false;
    }
}