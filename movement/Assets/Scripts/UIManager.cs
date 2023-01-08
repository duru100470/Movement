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
    [SerializeField]
    private GameObject resetWarningPanel;
    [SerializeField]
    private GameObject resetDonePanel;
    [SerializeField]
    private GameObject clearPanel;
    [SerializeField]
    private GameObject failPanel;
    [SerializeField]
    private GameObject creditPanel;
    [SerializeField]
    private PlayerDataManager pdm;
    [SerializeField]
    private TurnManager tm;

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

        //SoundManager.Inst.PlayEffectSound(SOUND_NAME.EQUIP_SOUND, 1f, 1f);

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

        SoundManager.Inst.PlayEffectSound(SOUND_NAME.BREAK_SOUND, 1f, 1f);
        Destroy(target.CurTile.gameObject);
    }

    public void LoadMainScene() {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadStageSelectScene() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void LoadStageScene() {
        SceneManager.LoadScene(gameObject.GetComponent<Level>().level + 1);
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
    public void SelectStopTile() {
        selectedTilePrefab = prefabList[8];
    }
    public void SelectTileByIndex(int index)
    {
        selectedTilePrefab = prefabList[index];
    }

    public void showResetWarning() {
        SoundManager.Inst.ChangeBGM(SOUND_NAME.MAIN_BGM, 0.15f);
        resetWarningPanel.SetActive(true);
    }
    public void Operate1() {
        resetWarningPanel.SetActive(false);
        resetDonePanel.SetActive(true);
        pdm.ResetData();
    }

    public void Operate2()
    {
        SoundManager.Inst.ChangeBGM(SOUND_NAME.MAIN_BGM, 0.3f);
        resetWarningPanel.SetActive(false);
    }
    public void Operate3() {
        SoundManager.Inst.ChangeBGM(SOUND_NAME.MAIN_BGM, 0.3f);
        resetDonePanel.SetActive(false);
    }
    public void Operate4() {
        clearPanel.SetActive(false);
        SceneManager.LoadScene("LevelSelect");
    }

    public void Operate5() {
        clearPanel.SetActive(false);
        int level = SceneManager.GetActiveScene().buildIndex;
        if (level < 21) SceneManager.LoadScene(level + 1);
        else SceneManager.LoadScene("LevelSelect");
    }
    public void Operate6()
    {
        failPanel.SetActive(false);
        SceneManager.LoadScene("LevelSelect");
    }

    
    public void SetSpeed(float speed)
    {
        SoundManager.Inst.PlayEffectSound(SOUND_NAME.CHECK_SOUND, 1f, 2f);
        GameManager.Inst.gameSpeed = speed;
    }

    public void ShowClearPanel() {
        SoundManager.Inst.PauseBGM(SOUND_NAME.LEVEL_BGM);
        clearPanel.SetActive(true);
    }

    public void ShowFailPanel()
    {
        SoundManager.Inst.PauseBGM(SOUND_NAME.LEVEL_BGM);
        failPanel.SetActive(true);
    }

    public void ShowCreditPanel()
    {
        SoundManager.Inst.ChangeBGM(SOUND_NAME.MAIN_BGM, 0.15f);
        creditPanel.SetActive(true);
    }

    public void HideCreditPanel()
    {
        SoundManager.Inst.ChangeBGM(SOUND_NAME.MAIN_BGM, 0.3f);
        creditPanel.SetActive(false);
    }
}
