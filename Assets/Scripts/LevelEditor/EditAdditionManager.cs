using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditAdditionManager : Singleton<EditAdditionManager> 
{
    [HideInInspector] public LevelEditManager editor;

    public void AddGround()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        (int q, int r, _) = GridLayoutRules.GetTileCoordsFromPositionFlatTop(editor.LevelBeingEdited.layoutData, worldPos);

        GridTile tileToPlace = new(q, r);
        PlaceTileCommand c = new PlaceTileCommand(tileToPlace);
        c.Execute();
    }

    public void AddUnit()
    {

    }
}
