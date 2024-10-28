using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditAdditionManager : Singleton<EditAdditionManager> 
{
    [HideInInspector] public LevelEditManager editor;

    public void AddGround()
    {
        (int q, int r) = GetCoordsFromMousePos();

        GridTile tileToPlace = new(q, r);
        PlaceTileCommand c = new PlaceTileCommand(tileToPlace);
        c.Execute();
    }

    public void AddUnit()
    {
        (int q, int r) = GetCoordsFromMousePos();

        UnitData newUnitData = ScriptableObject.CreateInstance<UnitData>();
        PlaceUnitCommand c = new PlaceUnitCommand(q, r, newUnitData);
        c.Execute();
    }

    private (int q, int r) GetCoordsFromMousePos()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        (int _q, int _r, _) = GridLayoutRules.GetTileCoordsFromPositionFlatTop(editor.LevelBeingEdited.layoutData, worldPos);
        return (_q, _r);
    }
}
