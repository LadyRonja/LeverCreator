using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeighbourDirections { NW, N, NE, SE, S, SW}


public class GridInformant : Singleton<GridInformant>
{
    LevelData activeLevel;
    Dictionary<NeighbourDirections, (int q, int r)> relationalCoordsLookupTable = new(){
                                                    { NeighbourDirections.NW, (-1, 1) },
                                                    { NeighbourDirections.N, (0, 1) },
                                                    { NeighbourDirections.NE, (1, 0) },
                                                    { NeighbourDirections.SE, (1, -1) },
                                                    { NeighbourDirections.S, (0, -1) },
                                                    { NeighbourDirections.SW, (-1, 0) }
    };

    private void Start()
    {
        if (activeLevel == null)
        {
            Debug.Log("Grid Informant requires data setup");
        }
    }


    public void SetActiveGrid(LevelData levelToSetActive)
    {
        activeLevel = levelToSetActive;

        Debug.Log("Unit names!");
        foreach (var item in activeLevel.units.Keys)
        {
            _ = activeLevel.units.TryGetValue(item, out Unit unit);

            Debug.Log(unit.transform.name);
        }
    }

    #region Tile data
    public bool TryGetNeighbourTile(int q, int r, NeighbourDirections inDirection, out GridTile neighbour)
    {
        neighbour = null;
        if(activeLevel == null) { return false; }

        (int qDir, int rDir) = relationalCoordsLookupTable.GetValueOrDefault(inDirection);
        string neighbourCoords = GridTile.GetStringFromCoords(q + qDir, r + rDir);
        return activeLevel.tiles.TryGetValue(neighbourCoords, out neighbour);
    }

    public bool TryGetNeighbourTile(GridTile ofTile, NeighbourDirections inDirection, out GridTile neighbour)
    {
        return TryGetNeighbourTile(ofTile.q, ofTile.r, inDirection, out neighbour);
    }

    public List<GridTile> GetAllNeighbourTiles(int q, int r)
    {
        if(activeLevel == null) { return new(); }

        List<GridTile> output = new();
        NeighbourDirections[] directions = System.Enum.GetValues(typeof(NeighbourDirections)) as NeighbourDirections[];

        foreach (var dir in directions)
        {
            if(TryGetNeighbourTile(q, r, dir, out GridTile foundNeighbour))
            {
                output.Add(foundNeighbour);
            }
        }

        return output;
    }
    public List<GridTile> GetAllNeighbourTiles(GridTile ofTile)
    {
        return GetAllNeighbourTiles(ofTile.q, ofTile.r);
    }

    public bool TileExists(int q, int r, out GridTile tile)
    {
        tile = null;
        if(activeLevel== null) { return false; }

        return activeLevel.tiles.TryGetValue(GridTile.GetStringFromCoords(q, r), out tile);
    }
    #endregion

    #region Unit data
    public bool TryGetUnit(int q, int r, out Unit unit)
    {
        unit = null;
        if(activeLevel == null) { return false; }

        return activeLevel.units.TryGetValue(GridTile.GetStringFromCoords(q ,r), out unit);
    }

    public bool TryGetUnit(GridTile onTile, out Unit unit)
    {
        unit = null;
        return TryGetUnit(onTile.q, onTile.r, out unit);
    }
    #endregion
}
