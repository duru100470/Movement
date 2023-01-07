using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> prefabList;
    private GameObject selectedTilePrefab;

    private void Update()
    {
        // Get Mouse Input
        // Transform mouse screen position to coordinate
    }

    private void AddTile(Coordinate pos)
    {
        TileHolder target;
        
        if (!TileManager.Inst.TileHolderDict.TryGetValue(pos, out target)) return;
        if (!target.CanPlaceTile || target.CurTile != null) return;

        var obj = Instantiate(selectedTilePrefab);
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

}
