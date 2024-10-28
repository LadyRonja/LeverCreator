using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GridLayoutRules;

public enum ClickMode { TOGGLE, ADD, REMOVE };
public enum GridLayers { TERRAIN, UNIT};

public delegate void EditMode();

public class LevelEditManager : Singleton<LevelEditManager>
{
    public ClickMode editMethod = ClickMode.ADD;
    public GridLayers editLayer = GridLayers.TERRAIN;
    Dictionary<(ClickMode, GridLayers), EditMode> editModes = new();

    [HideInInspector] public EditAdditionManager additionManager;
    [HideInInspector] public EditRemovalManager removalManager;

    LevelData levelBeingEdited = null;
    public LevelData LevelBeingEdited { get => levelBeingEdited; private set => levelBeingEdited = value; }
    
    [SerializeField] GameObject tempTilePrefab; //temp solution
    List<GameObject> levelObjects = new();

    protected override void Awake()
    {
        base.Awake();

        SetUpEditModes();
        SetUpLevel();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown((int)MouseButton.Left))
        {
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

        editModes.Clear();
        editModes.Add((ClickMode.ADD, GridLayers.TERRAIN), () => additionManager.AddGround());
        editModes.Add((ClickMode.REMOVE, GridLayers.TERRAIN), () => removalManager.RemoveGround());
    }

    private void SetUpLevel()
    {
        levelBeingEdited = new();

        Vector2 startPos = transform.position;
        GameObject temp = Instantiate(tempTilePrefab, startPos, Quaternion.identity, this.transform);
        SpriteRenderer sr = temp.GetComponent<SpriteRenderer>();
        float tileWidth = sr.bounds.size.x;
        float tileHeight = sr.bounds.size.y;
        GridLayoutRules.LayoutData layoutData = new GridLayoutRules.LayoutData(startPos, tileWidth, tileHeight);
        Destroy(temp);

        levelBeingEdited = new();
        levelBeingEdited.layoutData = layoutData;

        levelObjects = new();
    }

    public void LoadLevelFromData(string levelData)
    {
        UnloadActiveLevel();

        LevelData levelToLoad = (LevelData)JsonConvert.DeserializeObject<LevelData>(levelData);

        levelBeingEdited = levelToLoad;
        foreach (GridTile t in levelToLoad.tiles.Values)
        {
            Vector2 tilePos = GridLayoutRules.GetPositionForFlatTopTile(levelToLoad.layoutData, t.q, t.r);

            GameObject tile = Instantiate(tempTilePrefab, tilePos, Quaternion.identity, this.transform);
            tile.transform.name = t.coords;
            levelObjects.Add(tile);
        }
    }

    private void UnloadActiveLevel()
    {
        foreach (GameObject go in levelObjects)
        {
            Destroy(go);
        }

        levelBeingEdited = null;
    }

    public void CreateTile(int q, int r, GridTile tile)
    {
        RemoveTile(q, r);

        levelBeingEdited.tiles.Add(tile.coords, tile);

        Vector2 tilePos = GridLayoutRules.GetPositionForFlatTopTile(levelBeingEdited.layoutData, q, r);
        GameObject newTileObj = Instantiate(tempTilePrefab, tilePos, Quaternion.identity, this.transform);
        newTileObj.transform.name = tile.coords;

        levelObjects.Add(newTileObj);
    }

    public void RemoveTile(int q, int r)
    {
        string tileName = GridTile.GetStringFromCoords(q, r);
        GameObject obj = GameObject.Find(tileName);

        if(levelObjects.Contains(obj))
        {
            levelObjects.Remove(obj);
        }

        Destroy(obj);

        if(levelBeingEdited.tiles.ContainsKey(tileName)){
            levelBeingEdited.tiles.Remove(tileName);
        }
    }


}
