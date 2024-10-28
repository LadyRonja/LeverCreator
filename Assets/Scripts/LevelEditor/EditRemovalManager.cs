using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditRemovalManager : Singleton<EditRemovalManager>
{
    [HideInInspector] public LevelEditManager editor;

    public void RemoveGround()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        (int q, int r, _) = GridLayoutRules.GetTileCoordsFromPositionFlatTop(editor.LevelBeingEdited.layoutData, worldPos);

        RemoveTileCommand c = new(q, r);
        c.Execute();
    }
}
