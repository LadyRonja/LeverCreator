using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public Dictionary<string, GridTile> tiles = new();
    public Dictionary<string, Unit> units = new();
    public string tileOrigo = "";
    public GridLayoutRules.LayoutData tileData;
}
