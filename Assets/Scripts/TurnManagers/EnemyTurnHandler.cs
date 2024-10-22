using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTurnHandler : Singleton<EnemyTurnHandler>
{
    public void StartTurn()
    {
        StartCoroutine(MoveEachAIUnit());
    }

    public void EndTurn()
    {
        TurnManger.Instance.SetTurn(TurnManger.TurnTakers.PLAYER);
    }

    private IEnumerator MoveEachAIUnit()
    {
        List<Unit> allAIUnits = FindObjectsOfType<Unit>().Where(u => !u.data.controlledByPlayer).ToList();

        foreach (Unit unit in allAIUnits)
        {
            // Find target
            List<Unit> allPlayerUnits = FindObjectsOfType<Unit>().Where(u => u.data.controlledByPlayer).ToList();
            if (allPlayerUnits.Count == 0) { Debug.LogError("No player units found!"); continue; }

            // See how far away the first one is
            Unit nearestFoundUnit = allPlayerUnits[0];
            _ = GridInformant.Instance.TryGetTileFromUnit(unit, out GridTile fromTile);
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

                yield return null;
            }
            yield return StartCoroutine(MovePath(unit, path));

        }

        EndTurn();
        yield return null;
    }

    private IEnumerator MovePath(Unit unitToMove, List<GridTile> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            yield return StartCoroutine(MoveStep(unitToMove, path[i]));
        }

        yield return null;
    }

    protected IEnumerator MoveStep(Unit unitToMove, GridTile toTile)
    {
        Vector3 startPos = unitToMove.transform.position;
        Vector3 endPos = GridInformant.Instance.GetPositionWorldFromTile(toTile);

        float timeToMove = 0.5f;
        float timePassed = 0;

        while (timePassed < timeToMove)
        {
            unitToMove.transform.position = Vector3.Lerp(startPos, endPos, (timePassed / timeToMove));

            timePassed += Time.deltaTime;
            yield return null;
        }

        unitToMove.transform.position = endPos;
        if (!ActiveLevelManager.Instance.TryMoveUnit(unitToMove, toTile))
        {
            Debug.LogError("Unable to move unit along predicted path!");
        }

        yield return null;
    }
}
