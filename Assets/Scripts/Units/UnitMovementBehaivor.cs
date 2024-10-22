using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UnitMovementBehaivor
{
   public static List<GridTile> GetPathToNearestPlayerControlledUnit(Unit fromUnit)
   {
        List<GridTile> output = new();

        // Find target
        List<Unit> allPlayerUnits = ActiveLevelManager.Instance.ActiveLevel.units.Values.Where(u => u.data.controlledByPlayer).ToList();

        if (allPlayerUnits.Count == 0) { return output; }

        // See how far away the first one is
        Unit nearestFoundUnit = allPlayerUnits[0];
        _ = GridInformant.Instance.TryGetTileFromUnit(fromUnit, out GridTile fromTile);
        _ = GridInformant.Instance.TryGetTileFromUnit(nearestFoundUnit, out GridTile toTile);

        int nearestDistance = int.MaxValue;
        List<GridTile> path = Pathfinder.FindPath(fromTile, toTile);
        if (path != null)
            nearestDistance = path.Count;

        // Compare the the currently closest unit to each other unit, if the distance to another unit is shorter,
        // update which one is beign measured from
        for (int i = 1; i < allPlayerUnits.Count; i++)
        {
            int dist = int.MaxValue;
            _ = GridInformant.Instance.TryGetTileFromUnit(allPlayerUnits[i], out toTile);

            List<GridTile> newPath = Pathfinder.FindPath(fromTile, toTile);
            if (newPath != null)
                dist = newPath.Count;
            else
                dist = int.MaxValue;

            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                path = newPath;
                nearestFoundUnit = allPlayerUnits[i];
            }
        }

        if(path != null && path.Count != 0)
        {
            output = path;
        }
        
        return output;
   }
}
