using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    public Coordinate Pos { get; set; }

    public Tile CurTile { get; set; }
    [SerializeField]
    private bool canPlaceTile;
    public bool CanPlaceTile => canPlaceTile;

    private void Awake()
    {
        CurTile = GetComponentInChildren<Tile>();
        Pos = Coordinate.WorldPointToCoordinate(transform.position);
    }
}