using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField]
    private TILE_TYPE tileType;
    public TILE_TYPE TileType => tileType;
    public bool isRunning { get; set; } = false;
    public abstract void RunCommand(Ground ground);
}