using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.FilePathAttribute;

public class DeselectCommand : Command
{
    HashSet<(int q, int r)> oldLocations;
    HashSet<GridLayers> oldLayers;

    delegate void HighlightFunction(int q, int r);
    static Dictionary<GridLayers, HighlightFunction> LayerToHighligthFunctionLookUp = new()
    {
        { GridLayers.UNIT, SelectorHighlightManager.Instance.SetHighlightUnit },
        { GridLayers.TERRAIN, SelectorHighlightManager.Instance.SetHighlightTerrain }
    };

    public override void Execute()
    {
        base.Execute();

        oldLocations = EditSelectionManager.Instance.selectedLocations;
        oldLayers = EditSelectionManager.Instance.selectedLayers;

        EditSelectionManager.Instance.selectedLocations = new();
        EditSelectionManager.Instance.selectedLayers = new();

        SelectorHighlightManager.Instance.DeselectAll();
    }
    public override void Undo()
    {
        if (oldLayers == null || oldLayers.Count == 0) { return; }

        foreach (var pos in oldLocations)
        {
            foreach (var layer in oldLayers)
            {
                if (LayerToHighligthFunctionLookUp.TryGetValue(layer, out HighlightFunction functionToCall))
                {
                    functionToCall(pos.q, pos.r);
                }
            }
        }

        EditSelectionManager.Instance.selectedLocations = oldLocations;
        EditSelectionManager.Instance.selectedLayers = oldLayers;
    }
}
