using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    public Coordinate Pos { get; set; }
    private Tile curTile;
    public Tile CurTile => curTile;
}