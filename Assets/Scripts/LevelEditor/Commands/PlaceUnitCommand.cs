using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceUnitCommand : Command
{
    public PlaceUnitCommand()
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
