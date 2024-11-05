using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeselectCommand : Command
{


    public override void Execute()
    {
        base.Execute();


    }
    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}
