using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EditUnitDataManager : Singleton<EditUnitDataManager>
{
    public void DisplayData(UnitData ud)
    {
        FieldInfo[] myFieldInfo;
        Type myType = ud.GetType();
        myFieldInfo = myType.GetFields(BindingFlags.Instance | BindingFlags.Public);

        EditorUnitDataUI.Instance.ClearAllDataCells();
        for (int i = 0; i < myFieldInfo.Length; i++)
        {
            if (myFieldInfo[i].Name == nameof(ud.unitIDforClonedData)) { continue; }
            if (myFieldInfo[i].Name == nameof(ud.AssetID)) { continue; }

            var result = ud.GetType().GetField(myFieldInfo[i].Name).GetValue(ud);

            EditorUnitDataUI.Instance.CreateNewDataCell(myFieldInfo[i].Name, myFieldInfo[i].FieldType.ToString(), result.ToString());
        }
    }
}
