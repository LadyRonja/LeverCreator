using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int debugWidth = 10;
    public int debugHeight = 15;
    public GameObject tilePrefab;
    public GameObject unitPrefab;

    Dictionary<string, GridTile> tiles = new();
    Dictionary<string, Unit> units = new();

    Vector3 startPos;
    float tileWidth;
    float tileHeight;
    GridLayoutRules.LayoutData tileData;

    private void Start()
    {
        SetUp();
        GenerateGrid();
        tempGenerateUnits();

        LevelData activeLevel = ScriptableObject.CreateInstance<LevelData>();
        activeLevel.tiles = tiles;
        activeLevel.units = units;
        activeLevel.tileData = tileData;

        GridInformant.Instance.SetActiveGrid(activeLevel);
        ActiveLevelManager.Instance.SetActiveGrid(activeLevel);
    }

    private void GenerateGrid()
    {
        tiles = new();

        for (int q = 0; q < debugWidth; q++)
        {
            for (int r = 0; r < debugHeight; r++)
            {
                Vector2 tilePos = GridLayoutRules.GetPositionForFlatTopTile(tileData, q, r);
                GridTile newTile = new GridTile(q, r);

                GameObject tile = Instantiate(tilePrefab, tilePos, Quaternion.identity, this.transform);

                tile.transform.name = $"{newTile.coords}";
                tiles.Add(newTile.coords, newTile);
            }
        }
    }

    private void tempGenerateUnits()
    {
        units = new();

        for (int q = 0; q < debugWidth; q++)
        {
            for (int r = 0; r < debugHeight; r++)
            {
                if (q != r) continue;

                Vector2 unitPos = GridLayoutRules.GetPositionForFlatTopTile(tileData, q, r);
                GameObject unitObj = Instantiate(unitPrefab, unitPos, Quaternion.identity);
                Unit unitScr = unitObj.GetComponent<Unit>();
                
                string unitPosString = GridTile.GetStringFromCoords(q, r);
                unitScr.unitID = Guid.NewGuid().ToString();
                
                unitObj.transform.name = unitScr.unitID;
                units.Add(unitPosString, unitScr);
            }
        }
    }

    private void GenerateFromJson(string level)
    {
        var dictTiles = (Dictionary<string, GridTile>)JsonConvert.DeserializeObject<Dictionary<string, GridTile>>(level);
        Vector3 startPos = transform.position;
        GameObject temp = Instantiate(tilePrefab, startPos, Quaternion.identity, this.transform);
        SpriteRenderer sr = temp.GetComponent<SpriteRenderer>();
        float tileWidth = sr.bounds.size.x;
        float tileHeight = sr.bounds.size.y;
        GridLayoutRules.LayoutData tileData = new GridLayoutRules.LayoutData(startPos, tileWidth, tileHeight);
        Destroy(temp);

        foreach (GridTile t in dictTiles.Values)
        {
            Vector2 tilePos = GridLayoutRules.GetPositionForFlatTopTile(tileData, t.q, t.r);

            GameObject tile = Instantiate(tilePrefab, tilePos, Quaternion.identity, this.transform);
            tile.transform.name = $"Hex ({t.coords})";
        }
    }

    private void SetUp()
    {
        startPos = transform.position;
        GameObject temp = Instantiate(tilePrefab, startPos, Quaternion.identity, this.transform);
        SpriteRenderer sr = temp.GetComponent<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;
        tileHeight = sr.bounds.size.y;
        tileData = new GridLayoutRules.LayoutData(startPos, tileWidth, tileHeight);
        Destroy(temp);
    }
}
