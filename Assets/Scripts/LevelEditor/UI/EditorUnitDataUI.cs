using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorUnitDataUI : Singleton<EditorUnitDataUI>
{
    [SerializeField] EditorVariableDataCell dataCellPrefab;
    [Space]
    [SerializeField] Transform variableCellContainer;
    [SerializeField] Button saveChangesBtn;
    [SerializeField] Button discardChangesBtn;

    public void CreateNewDataCell(string varName, string varType, string varValue)
    {
        EditorVariableDataCell newCell = Instantiate(dataCellPrefab, variableCellContainer);

        string[] typeNamespaceSeperators = varType.Split('.');

        newCell.nameLabel.text = varName;
        newCell.typeLabel.text = typeNamespaceSeperators[^1];
        newCell.variableInputField.text = varValue;
    }

    public void ClearAllDataCells()
    {
        foreach (Transform child in variableCellContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
