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

    private void Start()
    {
        saveChangesBtn.onClick.AddListener(delegate { EditUnitDataManager.Instance.UpdateData(); });
        discardChangesBtn.onClick.AddListener(delegate { EditUnitDataManager.Instance.DiscardCustomData(); });
    }

    public EditorVariableDataCell CreateNewDataCell(string varName, string varType, string varValue)
    {
        EditorVariableDataCell newCell = Instantiate(dataCellPrefab, variableCellContainer);

        newCell.fullTypeName = varType;
        string[] typeNamespaceSeperators = varType.Split('.');

        newCell.nameLabel.text = varName;
        newCell.typeLabel.text = typeNamespaceSeperators[^1];
        newCell.variableInputField.text = varValue;

        return newCell;
    }

    public void ClearAllDataCells()
    {
        foreach (Transform child in variableCellContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
