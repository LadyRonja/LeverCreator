using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCommand : Command
{
    HashSet<(int q, int r)> locations;
    HashSet<GridLayers> layers;

    HashSet<(int q, int r)> oldLocations;
    HashSet<GridLayers> oldLayers;

    delegate void HighlightFunction(int q, int r);
    static Dictionary<GridLayers, HighlightFunction> LayerToHighligthFunctionLookUp = new()
    {
        { GridLayers.UNIT, SelectorHighlightManager.Instance.SetHighlightUnit },
        { GridLayers.TERRAIN, SelectorHighlightManager.Instance.SetHighlightTerrain }
    };

    public SelectCommand(HashSet<(int q, int r)> locations, HashSet<GridLayers> layers)
    {
        this.locations = locations;
        this.layers = layers;
    }

    public override void Execute()
    {
        base.Execute();

        oldLocations = EditSelectionManager.Instance.selectedLocations;
        oldLayers = EditSelectionManager.Instance.selectedLayers;

        SelectorHighlightManager.Instance.DeselectAll();

        foreach (var pos in locations)
        {
            foreach (var layer in layers)
            {
                if(LayerToHighligthFunctionLookUp.TryGetValue(layer, out HighlightFunction functionToCall))
                {
                    functionToCall(pos.q, pos.r);
                }
            }
        }

        EditSelectionManager.Instance.selectedLocations = locations;
        EditSelectionManager.Instance.selectedLayers = layers;
    }
    public override void Undo()
    {
        SelectorHighlightManager.Instance.DeselectAll();

        if(oldLayers == null || oldLayers.Count == 0) { return; }

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
