using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public Dictionary<string, GridTile> tiles = new();
    public Dictionary<string, Unit> units = new();
    public string tileOrigo = "";
    public GridLayoutRules.LayoutData layoutData;
}
