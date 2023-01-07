using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabList;
    [SerializeField]
    private GameObject selectedTilePrefab;

    private void Update()
    {
        // Get Mouse Input
        // Transform mouse screen position to coordinate

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedTilePrefab == null) return;
            var tmp = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            var pos = Coordinate.WorldPointToCoordinate(tmp);

            Debug.Log($"{pos.X} {pos.Y}");

            AddTile(pos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var tmp = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            var pos = Coordinate.WorldPointToCoordinate(tmp);

            DeleteTile(pos);
        }
    }

    private void AddTile(Coordinate pos)
    {
        TileHolder target;

        if (!TileManager.Inst.TileHolderDict.TryGetValue(pos, out target)) return;
        if (!target.CanPlaceTile || target.CurTile != null) return;

        var obj = Instantiate(selectedTilePrefab);
        obj.transform.parent = target.transform;
        obj.transform.position = obj.transform.parent.position;
        target.CurTile = obj.GetComponent<Tile>();
    }

    private void DeleteTile(Coordinate pos)
    {
        TileHolder target;

        if (!TileManager.Inst.TileHolderDict.TryGetValue(pos, out target)) return;
        if (!target.CanPlaceTile || target.CurTile == null) return;

        Destroy(target.CurTile.gameObject);
    }
}
