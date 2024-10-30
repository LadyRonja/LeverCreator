using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class GridGenerator : Singleton<GridGenerator>
{
    public GameObject tilePrefab;
    public GameObject unitPrefab;

    [SerializeField] TextAsset levelDataFile;

    private void Start()
    {
        if (tilePrefab == null || unitPrefab == null)
        {
            Debug.LogError("GridGenerator Requires proper initialization");
        }

        /*
        LevelData levelToLoad = (LevelData)JsonConvert.DeserializeObject<LevelData>(levelDataFile.text);
        GridInformant.Instance.SetActiveGrid(levelToLoad);
        ActiveLevelManager.Instance.SetActiveGrid(levelToLoad);
        
        GenerateFromJson(levelDataFile.text);
        */
    }

    public void Initialize(GameObject tilePrefab, GameObject unitPrefab)
    {
        this.tilePrefab = tilePrefab;
        this.unitPrefab = unitPrefab;
    }

    public List<GameObject> GenerateFromJson(string level)
    {
        LevelData levelToLoad = (LevelData)JsonConvert.DeserializeObject<LevelData>(level);
        List<GameObject> levelObjects = new();

        foreach (GridTile t in levelToLoad.tiles.Values)
        {
            levelObjects.Add(CreateTile(t.q, t.r, t, levelToLoad.layoutData));
        }

        foreach (UnitData ud in levelToLoad.units.Values)
        {
            (int q, int r, _) = GridTile.GetCoordsFromCoordString(levelToLoad.units.First(u => u.Value == ud).Key);

            Vector2 tilePos = GridLayoutRules.GetPositionForFlatTopTile(levelToLoad.layoutData, q, r);
            GameObject unitObj = Instantiate(unitPrefab, tilePos, Quaternion.identity);
            Unit unitScr = unitObj.GetComponent<Unit>();
            unitScr.data = ud;
            unitScr.unitID = Guid.NewGuid().ToString();
            unitObj.name = unitScr.unitID;

            levelObjects.Add(unitObj);
        }

        return levelObjects;
    }

    public GameObject CreateTile(int q, int r, GridTile tile, GridLayoutRules.LayoutData layoutRules)
    {
        Vector2 tilePos = GridLayoutRules.GetPositionForFlatTopTile(layoutRules, q, r);
        GameObject newTileObj = Instantiate(tilePrefab, tilePos, Quaternion.identity, this.transform);
        newTileObj.transform.name = tile.coords;

        return newTileObj;
    }

    public GameObject CreateUnit(int q, int r, UnitData data, GridLayoutRules.LayoutData layoutRules)
    {
        string coords = GridTile.GetStringFromCoords(q, r);

        Vector2 worldPos = GridLayoutRules.GetPositionForFlatTopTile(layoutRules, q, r);
        GameObject unitObj = Instantiate(unitPrefab, worldPos, Quaternion.identity);
        Unit unitScr = unitObj.GetComponent<Unit>();
        unitScr.data = data;
        unitScr.unitID = Guid.NewGuid().ToString();
        unitObj.transform.name = unitScr.unitID;

        return unitObj;
    }
}
