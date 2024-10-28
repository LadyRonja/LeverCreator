using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementHandler : Singleton<UnitMovementHandler>
{
    public IEnumerator MovePath(Unit unitToMove, List<GridTile> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            yield return StartCoroutine(MoveStep(unitToMove, path[i]));
        }

        yield return null;
    }

    public IEnumerator MoveStep(Unit unitToMove, GridTile toTile)
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
        if (!ActiveLevelManager.Instance.TryMoveUnit(unitToMove.data, toTile))
        {
            Debug.LogError("Unable to move unit along predicted path!");
        }

        yield return null;
    }
}
