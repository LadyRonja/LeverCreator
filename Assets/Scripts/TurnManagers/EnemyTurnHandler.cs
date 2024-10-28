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
        Debug.Log("allAIUnits.Count: " + allAIUnits.Count);

        foreach (Unit unit in allAIUnits)
        {
            List<GridTile> pathToClosestPlayernit = UnitMovementBehaivor.GetPathToNearestPlayerControlledUnit(unit);

            yield return StartCoroutine(UnitMovementHandler.Instance.MovePath(unit, pathToClosestPlayernit));

        }

        EndTurn();
        yield return null;
    }
}
