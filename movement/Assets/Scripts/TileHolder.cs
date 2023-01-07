using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    public Coordinate Pos { get; set; }

    [SerializeField]
    private Tile curTile;
    public Tile CurTile => curTile;

    private void Awake()
    {
        curTile = GetComponentInChildren<Tile>();
        Pos = Coordinate.WorldPointToCoordinate(transform.position);
    }
}