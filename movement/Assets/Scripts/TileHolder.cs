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
    [SerializeField]
    private Sprite[] holderSprites;

    private void Awake()
    {
        if (GetComponentInChildren<Tile>() != null) CurTile = GetComponentInChildren<Tile>();
        Pos = Coordinate.WorldPointToCoordinate(transform.position);

        if(GetComponent<SpriteRenderer>() != null)
            this.GetComponent<SpriteRenderer>().sprite = canPlaceTile ? holderSprites[0] : holderSprites[1]; 

        if (CurTile == null) return;
        CurTile.GetComponent<SpriteRenderer>().color = 
            new Color(1f, 1f, 1f, .6f);
    }
}