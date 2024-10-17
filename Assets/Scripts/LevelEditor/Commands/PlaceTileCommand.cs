using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTileCommand : Command
{

    public PlaceTileCommand()
    {

    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}
