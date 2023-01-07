using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    private ENTITY_TYPE entityType;

    public ENTITY_TYPE EntityType => entityType;
    public Coordinate Pos { get; set; }

    private void Awake()
    {
        Pos = Coordinate.WorldPointToCoordinate(transform.position);
    }
}
