using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabList;
    [SerializeField]
    private GameObject selectedTilePrefab;

    private int selectedlevel;
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
        obj.transform.position = obj.transform.parent.position + new Vector3(0,0,-1);
        target.CurTile = obj.GetComponent<Tile>();
    }

    private void DeleteTile(Coordinate pos)
    {
        TileHolder target;

        if (!TileManager.Inst.TileHolderDict.TryGetValue(pos, out target)) return;
        if (!target.CanPlaceTile || target.CurTile == null) return;

        Destroy(target.CurTile.gameObject);
    }

    public void LoadMainScene() {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadCreditScene() {
        SceneManager.LoadScene("Credit");
    }

    public void LoadStageSelectScene() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadStageScene() {
        SceneManager.LoadScene(gameObject.GetComponent<Level>().level);
    }
    public void SelectUpTile() {
        selectedTilePrefab = prefabList[0];
    }
    public void SelectDownTile()
    {
        selectedTilePrefab = prefabList[1];
    }
    public void SelectLeftTile()
    {
        selectedTilePrefab = prefabList[2];
    }
    public void SelectRightTile()
    {
        selectedTilePrefab = prefabList[3];
    }
    public void SelectDoubleUpTile()
    {
        selectedTilePrefab = prefabList[4];
    }
    public void SelectDoubleDownTile()
    {
        selectedTilePrefab = prefabList[5];
    }
    public void SelectDoubleLeftTile()
    {
        selectedTilePrefab = prefabList[6];
    }
    public void SelectDoubleRightTile()
    {
        selectedTilePrefab = prefabList[7];
    }

}
