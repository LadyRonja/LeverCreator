using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTurnHandler : Singleton<PlayerTurnHandler>
{
    bool inputEnabled = true;
    Unit selectedUnit = null;

    void Update()
    {
        if (!inputEnabled) return;

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            OnLeftclick();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpaceDown();
        }
    }

    private void OnLeftclick()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(!GridInformant.Instance.TryGetTileFromWorldPos(worldPos, out GridTile clickedTile)) { return; } // If  a tile was not clicked, exit function

        // If there is no selected unit, try and set one
        if(selectedUnit == null)
        {
            //Debug.Log("Trying to set unit");
            TrySetSelectedUnit(clickedTile);
            return;
        }

        // If the selected unit is controlled by the player, try to move it
        if (selectedUnit.data.controlledByPlayer)
        {
            //Debug.Log("Trying to move unit");
            TryToMoveSelectedUnit(clickedTile);
            return;
        }
        else {
            //Debug.Log("Deselecting enemy unit");
            selectedUnit= null;
            return;
        }
    }

    private void OnSpaceDown()
    {
        EndTurn();
    }

    public void EndTurn()
    {
        inputEnabled = false;
        TurnManger.Instance.SetTurn(TurnManger.TurnTakers.ENEMY);
    }

    public void StartTurn()
    {
        inputEnabled = true;
    }

    private void TrySetSelectedUnit(GridTile fromTile)
    {
        _ = GridInformant.Instance.TryGetUnit(fromTile, out selectedUnit);
    }

    private void TryToMoveSelectedUnit(GridTile toTile)
    {
        inputEnabled = false;
        if(!GridInformant.Instance.TryGetTileFromUnit(selectedUnit, out GridTile fromTile)) { inputEnabled = true; return; }

        List<GridTile> path = Pathfinder.FindPath(fromTile, toTile);
        if(path == null) { inputEnabled = true; selectedUnit = null; return; }
        if(path.Count == 0) { inputEnabled = true; selectedUnit = null; return; }

        //Debug.Log("starting movement!");
        StartCoroutine(MovePath(path));
    }

    private IEnumerator MovePath(List<GridTile> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            yield return StartCoroutine(MoveStep(path[i]));
        }

        selectedUnit = null;
        inputEnabled = true;
        yield return null;
    }

    protected IEnumerator MoveStep(GridTile toTile)
    {
        Vector3 startPos = selectedUnit.transform.position;
        Vector3 endPos =  GridInformant.Instance.GetPositionWorldFromTile(toTile);

        float timeToMove = 0.5f;
        float timePassed = 0;

        while (timePassed < timeToMove)
        {
            selectedUnit.transform.position = Vector3.Lerp(startPos, endPos, (timePassed / timeToMove));

            timePassed += Time.deltaTime;
            yield return null;
        }

        selectedUnit.transform.position = endPos;
        if(!ActiveLevelManager.Instance.TryMoveUnit(selectedUnit, toTile))
        {
            Debug.LogError("Unable to move unit along predicted path!");
        }

        yield return null;
    }

}
