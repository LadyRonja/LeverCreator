using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum ClickMode { ADD, REMOVE, SELECT };
public enum GridLayers { TERRAIN, UNIT};

public delegate void EditMode();

public class LevelEditManager : Singleton<LevelEditManager>
{
    public ClickMode editMethod = ClickMode.ADD;
    public GridLayers editLayer = GridLayers.TERRAIN;
    Dictionary<(ClickMode, GridLayers), EditMode> editModes = new();

    [HideInInspector] public EditAdditionManager additionManager;
    [HideInInspector] public EditRemovalManager removalManager;
    [HideInInspector] public EditSelectionManager selectionManager;

    LevelData levelBeingEdited = null;
    public LevelData LevelBeingEdited { get => levelBeingEdited; private set => levelBeingEdited = value; }

    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject unitPrefab;
    List<GameObject> levelObjects = new();

    protected override void Awake()
    {
        base.Awake();

        SetUpEditModes();
        SetUpFreshStart();
        GridGenerator.Instance.Initialize(tilePrefab, unitPrefab);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            if (ClickIsAtEdge(Input.mousePosition)) { return; }

            if (editModes.TryGetValue((editMethod, editLayer), out EditMode currentMode))
            {
                currentMode();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            CommandManager.Instance.Undo();
        }
    }

    private void SetUpEditModes()
    {
        additionManager = EditAdditionManager.Instance;
        additionManager.editor = this;
        removalManager = EditRemovalManager.Instance;
        removalManager.editor = this;
        selectionManager = EditSelectionManager.Instance;
        selectionManager.editor = this;

        editModes.Clear();
        editModes.Add((ClickMode.ADD, GridLayers.TERRAIN), () => additionManager.AddGround());
        editModes.Add((ClickMode.REMOVE, GridLayers.TERRAIN), () => removalManager.RemoveGround());
        editModes.Add((ClickMode.SELECT, GridLayers.TERRAIN), () => selectionManager.SelectTerrain());

        editModes.Add((ClickMode.ADD, GridLayers.UNIT), () => additionManager.AddUnit());
        editModes.Add((ClickMode.REMOVE, GridLayers.UNIT), () => removalManager.RemoveUnit());
        editModes.Add((ClickMode.SELECT, GridLayers.UNIT), () => selectionManager.SelectUnit());
    }

    private void SetUpFreshStart()
    {
        levelBeingEdited = new();

        Vector2 startPos = transform.position;
        GameObject temp = Instantiate(tilePrefab, startPos, Quaternion.identity, this.transform);
        SpriteRenderer sr = temp.GetComponent<SpriteRenderer>();
        float tileWidth = sr.bounds.size.x;
        float tileHeight = sr.bounds.size.y;
        GridLayoutRules.LayoutData layoutData = new GridLayoutRules.LayoutData(startPos, tileWidth, tileHeight);
        Destroy(temp);

        levelBeingEdited = new();
        levelBeingEdited.layoutData = layoutData;

        levelObjects = new();

        GridInformant.Instance.SetActiveGrid(levelBeingEdited);
        ActiveLevelManager.Instance.SetActiveGrid(levelBeingEdited);
    }

    public void LoadLevelFromData(string levelJSON)
    {
        UnloadActiveLevel();

        GridGenerator.Instance.GenerateFromJson(levelJSON, out levelBeingEdited, out levelObjects);

        CommandManager.Instance.ClearHistory();
        GridInformant.Instance.SetActiveGrid(levelBeingEdited);
    }

    private void UnloadActiveLevel()
    {
        foreach (GameObject go in levelObjects)
        {
            Destroy(go);
        }

        levelBeingEdited = null;
    }

    public void SaveActiveLevel(string asName)
    {
        string stringyfiedLevelData = JsonConvert.SerializeObject(levelBeingEdited);

        File.WriteAllText(Application.dataPath + "/Resources/Levels/" + asName + ".txt", stringyfiedLevelData);

        AssetDatabase.Refresh();

        Debug.Log("Level Saved!");
    }

    private bool ClickIsAtEdge(Vector2 screenPos)
    {
        float cutOffPercentage = 0.85f;

        float rightCutOff = Screen.width * cutOffPercentage;
        float leftCutOff = Screen.width * (1f - cutOffPercentage);
        float upCutOff = Screen.height * cutOffPercentage;
        float downCutOff = Screen.height * (1f - cutOffPercentage);

        #region Debug
        /*
        Debug.Log($"Cutoff right: {rightCutOff}");
        Debug.Log($"Cutoff left: {leftCutOff}");
        Debug.Log($"Cutoff up: {upCutOff}");
        Debug.Log($"Cutoff down : {downCutOff}");

        Debug.Log($"screenPos.x: {screenPos.x}");
        Debug.Log($"screenPos.y: {screenPos.y}");
        */
        #endregion

        if (screenPos.x < leftCutOff || screenPos.x > rightCutOff) { return true; }
        if (screenPos.y < downCutOff || screenPos.y > upCutOff) { return true; }

        return false;

    }


    public void CreateTile(int q, int r, GridTile tile)
    {
        RemoveTile(q, r);
        levelBeingEdited.tiles.Add(tile.coords, tile);
        levelObjects.Add(GridGenerator.Instance.CreateTile(q, r, tile, levelBeingEdited.layoutData));
    }

    public void RemoveTile(int q, int r)
    {
        string tileName = GridTile.GetStringFromCoords(q, r);
        GameObject obj = GameObject.Find(tileName);

        if (levelObjects.Contains(obj))
        {
            levelObjects.Remove(obj);
        }

        Destroy(obj);

        if (levelBeingEdited.tiles.ContainsKey(tileName))
        {
            levelBeingEdited.tiles.Remove(tileName);
        }
    }

    public void CreateUnit(int q, int r, UnitData data)
    {
        RemoveUnit(q, r);

        string coords = GridTile.GetStringFromCoords(q, r);
        levelBeingEdited.units.Add(coords, data);

        levelObjects.Add(GridGenerator.Instance.CreateUnit(q, r, data, levelBeingEdited.layoutData));
    }

    public void RemoveUnit(int q, int r)
    {
        if (!GridInformant.Instance.TryGetUnit(q, r, out Unit unit))
        {
            return;
        }

        GameObject obj = unit.gameObject;

        if (levelObjects.Contains(obj))
        {
            levelObjects.Remove(obj);
        }

        string dataCoords = GridTile.GetStringFromCoords(q, r);

        if (levelBeingEdited.units.ContainsKey(dataCoords))
        {
            levelBeingEdited.units.Remove(dataCoords);
        }

        Destroy(obj);
    }

}
