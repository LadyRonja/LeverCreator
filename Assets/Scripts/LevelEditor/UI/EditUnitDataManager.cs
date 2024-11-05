using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditUnitDataManager : Singleton<EditUnitDataManager>
{
    public void DisplayData(UnitData ud)
    {
        Debug.Log(ud.unitIDforClonedData);
    }
}
