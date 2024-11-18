using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
public class UpdateUnitDataCommand : Command
{
    UnitData dataToUpdate;
    UnitData oldData;
    string keyOfData;

    public UpdateUnitDataCommand(UnitData dataToUpdate)
    {
        this.dataToUpdate = dataToUpdate;
        oldData = Object.Instantiate(dataToUpdate);
        keyOfData = LevelEditManager.Instance.LevelBeingEdited.units.FirstOrDefault(u => u.Value == dataToUpdate).Key;
    }

    public override void Execute()
    {
        base.Execute();

        if(keyOfData == null || keyOfData.Equals(string.Empty)) { return; }
        EditUnitDataManager.Instance.UpdateData(dataToUpdate, keyOfData);
    }

    public override void Undo()
    {
        if (keyOfData == null || keyOfData.Equals(string.Empty)) { return; }
        if (oldData.aquireDefaultValueOnLoad)
        {
            EditUnitDataManager.Instance.DiscardCustomData();
        }
        else
        {
            EditUnitDataManager.Instance.UpdateData(oldData, keyOfData);
        }
    }
}*/
